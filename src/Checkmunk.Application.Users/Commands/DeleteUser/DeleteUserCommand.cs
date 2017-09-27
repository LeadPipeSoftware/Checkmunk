using System;
using Checkmunk.Application.Common;
using MediatR;

namespace Checkmunk.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>, IAmCorrelatable
    {
        public DeleteUserCommand(Guid correlationGuid, string emailAddress)
        {
            this.CorrelationGuid = correlationGuid;
            this.EmailAddress = emailAddress;
        }

        public Guid CorrelationGuid { get; private set; }

        public string EmailAddress { get; private set; }
    }
}