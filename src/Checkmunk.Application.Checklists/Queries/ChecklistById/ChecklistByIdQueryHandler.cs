using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Contracts.Checklists.V1.Models;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Checklists.AggregateRoots;
using Checkmunk.Domain.Checklists.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Checkmunk.Application.Checklists.Queries.ChecklistById
{
    public class ChecklistByIdQueryHandler : IAsyncRequestHandler<ChecklistByIdQuery, ChecklistModel>
    {
        private readonly CheckmunkContext context;
        private readonly IMapper mapper;

        public ChecklistByIdQueryHandler(CheckmunkContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public Task<ChecklistModel> Handle(ChecklistByIdQuery message)
        {
            var checklist = EntityFrameworkQueryableExtensions.Include<Checklist, User>(context.Checklists, c => c.CreatedBy).Include(c => c.Items).FirstOrDefault(c => c.Id.Equals(message.Id));

            var model = this.mapper.Map<ChecklistModel>(checklist);

            return Task.FromResult<ChecklistModel>(model);
        }
    }
}
