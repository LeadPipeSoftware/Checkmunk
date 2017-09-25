using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Data.Contexts;
using MediatR;

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

        public Task<UserModel> Handle(UserByEmailAddressQuery message)
        {
            var user = context.Users.FirstOrDefault(u => u.EmailAddress.Equals(message.EmailAddress));

            var model = this.mapper.Map<UserModel>(user);

            return Task.FromResult<UserModel>(model);
        }
    }
}
