using AutoMapper;
using Checkmunk.Contracts.Users.V1.Models;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Users;
using Checkmunk.Domain.Users.AggregateRoots;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<UserModel> Handle(CreateUserCommand command)
        {
            var existingUser = await context.GetUserByEmailAddress(command.CreateUserModel.EmailAddress);

            if (existingUser != null)
            {
                logger.LogWarning(LoggingEvents.CREATE_USER, "A {Command} was received for {EmailAddress}, but that already exists", command, command.CreateUserModel.EmailAddress);

                return await Task.FromResult(mapper.Map<UserModel>(existingUser));
            }

            var newUser = new User(command.CreateUserModel.EmailAddress, command.CreateUserModel.FirstName ?? "", command.CreateUserModel.LastName ?? "");

            await context.AddAsync(newUser);

            await context.SaveChangesAsync();

            var readUser = await context.GetUserByEmailAddress(command.CreateUserModel.EmailAddress);

            return await Task.FromResult(mapper.Map<UserModel>(readUser));
        }
    }
}