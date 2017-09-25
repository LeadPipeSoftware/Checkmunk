using Checkmunk.Contracts.Users.V1.Models;
using MediatR;

namespace Checkmunk.Application.Users.Queries.UserByEmailAddress
{
    public class UserByEmailAddressQuery : IRequest<UserModel>, IRequest
    {
        public UserByEmailAddressQuery(string emailAddress)
        {
            this.EmailAddress = emailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}
