using AutoMapper;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Data.Contexts;
using MediatR;
using System.Threading.Tasks;

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

        public async Task<UserModel[]> Handle(AllUsersQuery message)
        {
            var users = await context.GetAllUsers();

            var model = this.mapper.Map<UserModel[]>(users);

            return await Task.FromResult(model);
        }
    }
}