using System;
using Entities.Commands;
using Entities.Events;
using MassTransit;

namespace FrameworkWebWithGenericHost
{
    public class UserState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Uri ResponseAddress { get; set; }
    }

    public class UserStateMachine : MassTransitStateMachine<UserState>
    {
        // Custom statuses for this saga
        public State CreatingUser { get; private set; }

        // Events/Commands used in this saga
        public Event<CreateUser> CreateUser { get; private set; }
        public Event<UserCreated> UserCreated { get; private set; }

        public UserStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Initially(
                When(CreateUser)
                    .Then(c => c.Saga.ResponseAddress = c.ResponseAddress)
                    .Publish(c => new CreateUserByName(c.Saga.CorrelationId, c.Message.Name))
                    .TransitionTo(CreatingUser));

            During(CreatingUser,
                When(UserCreated)
                .ThenAsync(async c =>
                {
                    var endpoint = await c.GetSendEndpoint(c.Saga.ResponseAddress);
                    await endpoint.Send(new UserCreated(c.Saga.CorrelationId, c.Message.User));
                }));

            SetCompletedWhenFinalized();
        }
    }


}