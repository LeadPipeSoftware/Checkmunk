using System;
using Checkmunk.Application.Common;
using Checkmunk.Contracts.Users.V1.Models;
using MediatR;

namespace Checkmunk.Application.Users.Queries.UserByEmailAddress
{
    public class UserByEmailAddressQuery : IRequest<UserModel>, IRequest, IAmCorrelatable
    {
        public UserByEmailAddressQuery(Guid correlationGuid, string emailAddress)
        {
            this.CorrelationGuid = correlationGuid;
            this.EmailAddress = emailAddress;
        }

        public Guid CorrelationGuid { get; private set; }

        public string EmailAddress { get; private set; }
    }
}
