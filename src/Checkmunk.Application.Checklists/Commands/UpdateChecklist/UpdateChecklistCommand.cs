using System;
using Checkmunk.Contracts.Checklists.V1.Models;
using MediatR;

namespace Checkmunk.Application.Checklists.Commands.UpdateChecklist
{
    public class UpdateChecklistCommand : IRequest<Unit>
    {
        public UpdateChecklistCommand(Guid id, UpdateChecklistModel updateChecklistModel)
        {
            this.Id = id;
            this.UpdateChecklistModel = updateChecklistModel;
        }

        public Guid Id { get; set; }

        public UpdateChecklistModel UpdateChecklistModel { get; set; }
    }
}
