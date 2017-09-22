using AutoMapper;
using Checkmunk.Application.Users.Commands;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Users;
using Checkmunk.Domain.Users.AggregateRoots;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Checkmunk.Application.Users.CommandHandlers
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
                logger.LogWarning(LoggingEvents.CREATE_USER, $"A request to create an existing user ({command.CreateUserModel.EmailAddress}) was received.");

                return Task.FromResult(mapper.Map<UserModel>(existingUser));
            }

            var newUser = new User(command.CreateUserModel.EmailAddress, command.CreateUserModel.FirstName ?? "", command.CreateUserModel.LastName ?? "");

            context.Add(newUser);

            context.SaveChangesAsync();

            var readUser = context.Users.FirstOrDefaultAsync(u => u.EmailAddress.Equals(command.CreateUserModel.EmailAddress));

            return Task.FromResult(mapper.Map<UserModel>(readUser.Result));
        }
    }
}