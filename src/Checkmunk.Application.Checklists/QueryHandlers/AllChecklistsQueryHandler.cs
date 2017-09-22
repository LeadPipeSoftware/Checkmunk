using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Application.Checklists.Queries;
using Checkmunk.Contracts.Checklists.V1.Models;
using Checkmunk.Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Checkmunk.Application.Checklists.QueryHandlers
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
            var checklists = context.Checklists.Include(c => c.CreatedBy).Include(c => c.Items).ToArray();

            var model = this.mapper.Map<ChecklistModel[]>(checklists);

			return Task.FromResult(model);
        }
    }
}
