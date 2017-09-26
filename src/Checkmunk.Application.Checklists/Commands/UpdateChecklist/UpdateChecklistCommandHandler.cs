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

        public async Task<Unit> Handle(UpdateChecklistCommand command)
        {
            var existingChecklist = await context.GetChecklistById(command.Id);

            if (existingChecklist == null)
            {
                logger.LogWarning(LoggingEvents.UPDATE_CHECKLIST, "A {Command} was received for {Id}, but that does not exist", command, command.Id);

                return await Task.FromResult(new Unit());
            }

            if (existingChecklist.Title != command.UpdateChecklistModel.Title)
            {
                existingChecklist.ChangeTitle(command.UpdateChecklistModel.Title);
            }

            // TODO: Update checklist items

            context.Checklists.Update(existingChecklist);

            await context.SaveChangesAsync();

            return await Task.FromResult(new Unit());
        }
    }
}