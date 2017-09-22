using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Application.Users.Queries;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Data.Contexts;
using MediatR;

namespace Checkmunk.Application.Users.QueryHandlers
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
            var users = context.Users.ToArray();

            var model = this.mapper.Map<UserModel[]>(users);

			return Task.FromResult(model);
        }
    }
}
