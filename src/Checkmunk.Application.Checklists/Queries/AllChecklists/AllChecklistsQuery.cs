using Checkmunk.Contracts.Checklists.V1.Models;
using MediatR;

namespace Checkmunk.Application.Checklists.Queries.AllChecklists
{
    public class AllChecklistsQuery : IRequest<ChecklistModel[]>, IRequest
    {
    }
}
