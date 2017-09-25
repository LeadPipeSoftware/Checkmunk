using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Checklists;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Application.Checklists.Commands.UpdateChecklist
{
    public class UpdateChecklistCommandHandler : IAsyncRequestHandler<UpdateChecklistCommand, Unit>
    {
        private readonly CheckmunkContext context;
        private readonly ILogger<UpdateChecklistCommandHandler> logger;
        private readonly IMapper mapper;

        public UpdateChecklistCommandHandler(CheckmunkContext context, IMapper mapper, ILogger<UpdateChecklistCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(UpdateChecklistCommand command)
        {
            var existingChecklist = context.Checklists.FirstOrDefault(checklist => checklist.Id.Equals(command.Id));

            if (existingChecklist == null)
            {
                LoggerExtensions.LogWarning(logger, LoggingEvents.UPDATE_CHECKLIST, $"A request to update a checklist that does not exist ({command.Id}) was received.");

                return Task.FromResult(new Unit());
            }

            if (existingChecklist.Title != command.UpdateChecklistModel.Title)
            {
                existingChecklist.ChangeTitle(command.UpdateChecklistModel.Title);
            }

            // TODO: Update checklist items

            context.Checklists.Update(existingChecklist);

            context.SaveChangesAsync();

            return Task.FromResult(new Unit());
        }
    }
}