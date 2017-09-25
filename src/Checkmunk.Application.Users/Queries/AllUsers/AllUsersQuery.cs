using Checkmunk.Contracts.Users.V1.Models;
using MediatR;

namespace Checkmunk.Application.Users.Queries.AllUsers
{
    public class AllUsersQuery : IRequest<UserModel[]>, IRequest
    {
    }
}
