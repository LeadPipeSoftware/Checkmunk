using Checkmunk.Contracts.Checklists.V1.Models;
using MediatR;

namespace Checkmunk.Application.Checklists.Commands.CreateChecklist
{
    public class CreateChecklistCommand : IRequest<ChecklistModel>, IRequest
    {
        public CreateChecklistCommand(CreateChecklistModel createChecklistModel)
        {
            this.CreateChecklistModel = createChecklistModel;
        }

        public CreateChecklistModel CreateChecklistModel { get; set; }
    }
}
