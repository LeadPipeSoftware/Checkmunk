using FluentValidation;

namespace Checkmunk.Application.Checklists.Commands.UpdateChecklist
{
    public class UpdateChecklistCommandValidator : AbstractValidator<UpdateChecklistCommand>
    {
        public UpdateChecklistCommandValidator()
        {
            DefaultValidatorExtensions.NotEmpty(RuleFor(command => command.Id));
            DefaultValidatorExtensions.NotNull(RuleFor(command => command.UpdateChecklistModel));
            DefaultValidatorExtensions.NotEmpty(RuleFor(command => command.UpdateChecklistModel.Title));
		}
    }
}
