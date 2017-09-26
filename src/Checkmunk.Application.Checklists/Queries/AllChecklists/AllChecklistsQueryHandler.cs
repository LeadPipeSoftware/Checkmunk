using AutoMapper;
using Checkmunk.Contracts.Checklists.V1.Models;
using Checkmunk.Data.Contexts;
using MediatR;
using System.Threading.Tasks;

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

        public async Task<ChecklistModel[]> Handle(AllChecklistsQuery message)
        {
            var checklists = await context.GetAllChecklists();

            var model = this.mapper.Map<ChecklistModel[]>(checklists);

            return await Task.FromResult(model);
        }
    }
}