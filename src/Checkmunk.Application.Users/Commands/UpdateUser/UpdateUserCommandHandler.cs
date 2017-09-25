using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IAsyncRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly CheckmunkContext context;
        private readonly ILogger<UpdateUserCommandHandler> logger;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(CheckmunkContext context, IMapper mapper, ILogger<UpdateUserCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(UpdateUserCommand command)
        {
            var existingUser = context.Users.FirstOrDefault(user => user.EmailAddress.Equals(command.EmailAddress));

            if (existingUser == null)
            {
                LoggerExtensions.LogWarning(logger, LoggingEvents.UPDATE_USER, $"A request to update a user that does not exist ({command.EmailAddress}) was received.");

                return Task.FromResult(new Unit());
            }

            if (existingUser.FirstName != command.UpdateUserModel.FirstName || existingUser.LastName != command.UpdateUserModel.LastName)
            {
                existingUser.ChangeName(command.UpdateUserModel.FirstName, command.UpdateUserModel.LastName);
            }

            context.Users.Update(existingUser);

            context.SaveChangesAsync();

            return Task.FromResult(new Unit());
        }
    }
}