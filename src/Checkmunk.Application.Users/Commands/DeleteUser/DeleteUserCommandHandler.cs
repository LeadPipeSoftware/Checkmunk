using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IAsyncRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly CheckmunkContext context;
        private readonly ILogger<DeleteUserCommandHandler> logger;
        private readonly IMapper mapper;

        public DeleteUserCommandHandler(CheckmunkContext context, IMapper mapper, ILogger<DeleteUserCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteUserCommand command)
        {
            var existingUser = await context.GetUserByEmailAddress(command.EmailAddress);

            if (existingUser == null)
            {
                logger.LogWarning(LoggingEvents.DELETE_USER, "A {Command} was received for {EmailAddress}, but that does not exist", command, command.EmailAddress);

                return await Task.FromResult(new Unit());
            }

            context.Remove(existingUser);

            await context.SaveChangesAsync();

            return await Task.FromResult(new Unit());
        }
    }
}