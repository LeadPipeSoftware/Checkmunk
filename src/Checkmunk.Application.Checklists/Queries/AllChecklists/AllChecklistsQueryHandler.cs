using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Contracts.Checklists.V1.Models;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Checklists.AggregateRoots;
using Checkmunk.Domain.Checklists.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Checkmunk.Application.Checklists.Queries.AllChecklists
{
    public class AllChecklistsQueryHandler : IAsyncRequestHandler<AllChecklistsQuery, ChecklistModel[]>
    {
        private readonly CheckmunkContext context;
        private readonly IMapper mapper;

        public AllChecklistsQueryHandler(CheckmunkContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public Task<ChecklistModel[]> Handle(AllChecklistsQuery message)
        {
            var checklists = EntityFrameworkQueryableExtensions.Include<Checklist, User>(context.Checklists, c => c.CreatedBy).Include(c => c.Items).ToArray();

            var model = this.mapper.Map<ChecklistModel[]>(checklists);

			return Task.FromResult<ChecklistModel[]>(model);
        }
    }
}
