using System;
using Checkmunk.Application.Common;
using Checkmunk.Contracts.Users.V1.Models;
using MediatR;

namespace Checkmunk.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Unit>, IAmCorrelatable
    {
        public UpdateUserCommand(Guid correlationGuid, string emailAddress, UpdateUserModel updateUserModel)
        {
            this.CorrelationGuid = correlationGuid;
            this.EmailAddress = emailAddress;
            this.UpdateUserModel = updateUserModel;
        }

        public Guid CorrelationGuid { get; private set; }

        public string EmailAddress { get; private set; }

        public UpdateUserModel UpdateUserModel { get; private set; }
    }
}