using Checkmunk.Application.Common;
using Checkmunk.Contracts.Users.V1.Models;
using MediatR;
using System;

namespace Checkmunk.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserModel>, IRequest, IAmCorrelatable
    {
        public CreateUserCommand(Guid correlationGuid, CreateUserModel createUserModel)
        {
            this.CorrelationGuid = correlationGuid;
            this.CreateUserModel = createUserModel;
        }

        public Guid CorrelationGuid { get; private set; }

        public CreateUserModel CreateUserModel { get; private set; }
    }
}