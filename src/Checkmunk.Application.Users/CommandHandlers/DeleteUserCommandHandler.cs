﻿using AutoMapper;
using Checkmunk.Application.Users.Commands;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Users;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Checkmunk.Application.Users.CommandHandlers
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

        public Task<Unit> Handle(DeleteUserCommand command)
        {
            var existingUser = context.Users.FirstOrDefault(user => user.EmailAddress.Equals(command.EmailAddress));

            if (existingUser == null)
            {
                logger.LogWarning(LoggingEvents.DELETE_USER, $"A request to delete a user that does not exist ({command.EmailAddress}) was received.");

                return Task.FromResult(new Unit());
            }

            context.Remove(existingUser);

            context.SaveChangesAsync();

            return Task.FromResult(new Unit());
        }
    }
}