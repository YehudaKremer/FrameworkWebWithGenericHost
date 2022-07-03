using FrameworkWebWithGenericHost.Consumers;
using FrameworkWebWithGenericHost.Controllers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;

namespace FrameworkWebWithGenericHost
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<UserController>();

                services.AddMassTransit(x =>
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host("localhost", "/", h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });

                        cfg.ConfigureEndpoints(context);
                    });

                    x.AddConsumer<UserConsumer>();

                    x.AddSagaStateMachine<UserStateMachine, UserState>()
                      .InMemoryRepository();
                });
            })
            .Build();

            GlobalConfiguration.Configuration.Services.Replace(
             typeof(IHttpControllerActivator),
             new MsDiHttpControllerActivator(host.Services as ServiceProvider));

            host.RunAsync();
        }
    }
}
