using System;
using MediatR;

namespace Checkmunk.Application.Checklists.Commands.DeleteChecklist
{
    public class DeleteChecklistCommand : IRequest<Unit>
    {
        public DeleteChecklistCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }
}
