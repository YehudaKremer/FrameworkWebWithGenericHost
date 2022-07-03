using System.Threading.Tasks;
using MassTransit;
using Entities.Commands;
using Entities.Models;
using Entities.Events;

namespace FrameworkWebWithGenericHost.Consumers
{
    public class UserConsumer : IConsumer<CreateUserByName>
    {
        public async Task Consume(ConsumeContext<CreateUserByName> context)
        {
            await Task.Delay(2000);
            await context.Publish(new UserCreated(context.Message.CorrelationId, new User(1, context.Message.Name)));
        }
    }
}