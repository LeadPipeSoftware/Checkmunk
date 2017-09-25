using FluentValidation;

namespace Checkmunk.Application.Checklists.Commands.DeleteChecklist
{
    public class DeleteChecklistCommandValidator : AbstractValidator<DeleteChecklistCommand>
    {
        public DeleteChecklistCommandValidator()
        {
            DefaultValidatorExtensions.NotEmpty(RuleFor(command => command.Id));
		}
    }
}
