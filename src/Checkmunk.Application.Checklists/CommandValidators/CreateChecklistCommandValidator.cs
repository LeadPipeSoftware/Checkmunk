using Checkmunk.Application.Checklists.Commands;
using FluentValidation;

namespace Checkmunk.Application.Checklists.CommandValidators
{
    public class CreateChecklistCommandValidator : AbstractValidator<CreateChecklistCommand>
    {
        public CreateChecklistCommandValidator()
        {
            RuleFor(command => command.CreateChecklistModel.Title).NotNull();
            RuleFor(command => command.CreateChecklistModel.CreatedBy).NotNull();
		}
    }
}
