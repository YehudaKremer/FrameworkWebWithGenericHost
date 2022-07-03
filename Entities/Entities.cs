using System;
using Entities.Models;

namespace Entities.Models
{
    public record User(int ID, string Name);
}

namespace Entities.Commands
{
    public record CreateUser(Guid CorrelationId, string Name);
    public record CreateUserByName(Guid CorrelationId, string Name);
}

namespace Entities.Events
{
    public record UserCreated(Guid CorrelationId, User User);
}