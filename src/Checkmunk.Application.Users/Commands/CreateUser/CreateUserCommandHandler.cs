using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Users;
using Checkmunk.Domain.Users.AggregateRoots;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IAsyncRequestHandler<CreateUserCommand, UserModel>
    {
        private readonly CheckmunkContext context;
        private readonly ILogger<CreateUserCommandHandler> logger;
        private readonly IMapper mapper;

        public CreateUserCommandHandler(CheckmunkContext context, IMapper mapper, ILogger<CreateUserCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public Task<UserModel> Handle(CreateUserCommand command)
        {
            var existingUser = context.Users.FirstOrDefault(user => user.EmailAddress.Equals(command.CreateUserModel.EmailAddress));

            if (existingUser != null)
            {
                LoggerExtensions.LogWarning(logger, LoggingEvents.CREATE_USER, $"A request to create an existing user ({command.CreateUserModel.EmailAddress}) was received.");

                return Task.FromResult<UserModel>(mapper.Map<UserModel>(existingUser));
            }

            var newUser = new User(command.CreateUserModel.EmailAddress, command.CreateUserModel.FirstName ?? "", command.CreateUserModel.LastName ?? "");

            context.Add(newUser);

            context.SaveChangesAsync();

            var readUser = EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<User>(context.Users, u => u.EmailAddress.Equals(command.CreateUserModel.EmailAddress));

            return Task.FromResult<UserModel>(mapper.Map<UserModel>(readUser.Result));
        }
    }
}