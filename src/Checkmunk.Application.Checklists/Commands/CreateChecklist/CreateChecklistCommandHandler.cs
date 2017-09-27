using System.Threading.Tasks;
using AutoMapper;
using Checkmunk.Contracts.Checklists.V1.Models;
using Checkmunk.Data.Contexts;
using Checkmunk.Domain.Checklists.AggregateRoots;
using Checkmunk.Domain.Checklists.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Application.Checklists.Commands.CreateChecklist
{
    public class CreateChecklistCommandHandler : IAsyncRequestHandler<CreateChecklistCommand, ChecklistModel>
    {
        private readonly CheckmunkContext context;
        private readonly ILogger<CreateChecklistCommandHandler> logger;
        private readonly IMapper mapper;

        public CreateChecklistCommandHandler(CheckmunkContext context, IMapper mapper, ILogger<CreateChecklistCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<ChecklistModel> Handle(CreateChecklistCommand command)
        {
            var user = await context.ChecklistUsers.FirstOrDefaultAsync(u => u.EmailAddress.Equals(command.CreateChecklistModel.CreatedBy.EmailAddress))
                ?? new User(command.CreateChecklistModel.CreatedBy.EmailAddress);

            var newChecklist = ChecklistFactory.Build()
                .WithTitle(command.CreateChecklistModel.Title)
                .ByUser(user)
                .Finish();

            foreach (var item in command.CreateChecklistModel.Items)
            {
                newChecklist.AddCheckbox(item.Text);
            }

            context.Add(newChecklist);

            await context.SaveChangesAsync();

            var readChecklist = await context.GetChecklistById(newChecklist.Id);

            return await Task.FromResult(mapper.Map<ChecklistModel>(readChecklist));
        }
    }
}