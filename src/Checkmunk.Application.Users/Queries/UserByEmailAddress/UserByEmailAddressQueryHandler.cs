using AutoMapper;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Data.Contexts;
using MediatR;
using System.Threading.Tasks;

namespace Checkmunk.Application.Users.Queries.UserByEmailAddress
{
    public class UserByEmailAddressQueryHandler : IAsyncRequestHandler<UserByEmailAddressQuery, UserModel>
    {
        private readonly CheckmunkContext context;
        private readonly IMapper mapper;

        public UserByEmailAddressQueryHandler(CheckmunkContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UserModel> Handle(UserByEmailAddressQuery message)
        {
            var user = await context.GetUserByEmailAddress(message.EmailAddress);

            var model = this.mapper.Map<UserModel>(user);

            return await Task.FromResult(model);
        }
    }
}