using System;
using Checkmunk.Contracts.Checklists.V1.Models;
using MediatR;

namespace Checkmunk.Application.Checklists.Queries
{
    public class ChecklistByIdQuery : IRequest<ChecklistModel>, IRequest
    {
        public ChecklistByIdQuery(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; private set; }
    }
}
