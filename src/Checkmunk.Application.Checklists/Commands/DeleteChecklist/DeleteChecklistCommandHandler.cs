using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Checklists;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Application.Checklists.Commands.DeleteChecklist
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

        public async Task<Unit> Handle(DeleteChecklistCommand command)
        {
            var existingChecklist = await context.GetChecklistById(command.Id);

            if (existingChecklist == null)
            {
                logger.LogWarning(LoggingEvents.DELETE_CHECKLIST, "A {Command} was received for {Id}, but that does not exist", command, command.Id);

                return await Task.FromResult(new Unit());
            }

            context.Remove(existingChecklist);

            await context.SaveChangesAsync();

            return await Task.FromResult(new Unit());
        }
    }
}