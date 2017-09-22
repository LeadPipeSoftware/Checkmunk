using Checkmunk.Application.Checklists.Commands;
using FluentValidation;

namespace Checkmunk.Application.Checklists.CommandValidators
{
    public class DeleteChecklistCommandValidator : AbstractValidator<DeleteChecklistCommand>
    {
        public DeleteChecklistCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
		}
    }
}
