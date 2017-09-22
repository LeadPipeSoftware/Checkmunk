using AutoMapper;
using Checkmunk.Application.Checklists.Commands;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Checklists;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Checkmunk.Application.Checklists.CommandHandlers
{
    public class DeleteChecklistCommandHandler : IAsyncRequestHandler<DeleteChecklistCommand, Unit>
    {
        private readonly CheckmunkContext context;
        private readonly ILogger<DeleteChecklistCommandHandler> logger;
        private readonly IMapper mapper;

        public DeleteChecklistCommandHandler(CheckmunkContext context, IMapper mapper, ILogger<DeleteChecklistCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(DeleteChecklistCommand command)
        {
            var existingChecklist = context.Checklists.FirstOrDefault(checklist => checklist.Id.Equals(command.Id));

            if (existingChecklist == null)
            {
                logger.LogWarning(LoggingEvents.DELETE_CHECKLIST, $"A request to delete a checklist that does not exist ({command.Id}) was received.");

                return Task.FromResult(new Unit());
            }

            context.Remove(existingChecklist);

            context.SaveChangesAsync();

            return Task.FromResult(new Unit());
        }
    }
}