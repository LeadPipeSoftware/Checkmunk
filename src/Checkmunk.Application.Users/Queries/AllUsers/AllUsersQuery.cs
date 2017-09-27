using System;
using Checkmunk.Application.Common;
using Checkmunk.Contracts.Users.V1.Models;
using MediatR;

namespace Checkmunk.Application.Users.Queries.AllUsers
{
    public class AllUsersQuery : IRequest<UserModel[]>, IRequest, IAmCorrelatable
    {
        public AllUsersQuery(Guid correlationGuid)
        {
            this.CorrelationGuid = correlationGuid;
        }

        public Guid CorrelationGuid { get; private set; }
    }
}
