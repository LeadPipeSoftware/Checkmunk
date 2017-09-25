using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Users.AggregateRoots;
using MediatR;

namespace Checkmunk.Application.Users.Queries.AllUsers
{
    public class AllUsersQueryHandler : IAsyncRequestHandler<AllUsersQuery, UserModel[]>
    {
        private readonly CheckmunkContext context;
        private readonly IMapper mapper;

        public AllUsersQueryHandler(CheckmunkContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public Task<UserModel[]> Handle(AllUsersQuery message)
        {
            var users = Enumerable.ToArray<User>(context.Users);

            var model = this.mapper.Map<UserModel[]>(users);

			return Task.FromResult<UserModel[]>(model);
        }
    }
}
