using FluentValidation;

namespace Checkmunk.Application.Checklists.Commands.CreateChecklist
{
    public class CreateChecklistCommandValidator : AbstractValidator<CreateChecklistCommand>
    {
        public CreateChecklistCommandValidator()
        {
            DefaultValidatorExtensions.NotNull(RuleFor(command => command.CreateChecklistModel.Title));
            DefaultValidatorExtensions.NotNull(RuleFor(command => command.CreateChecklistModel.CreatedBy));
		}
    }
}
