using Checkmunk.Application.Checklists.Commands;
using FluentValidation;

namespace Checkmunk.Application.Checklists.CommandValidators
{
    public class UpdateChecklistCommandValidator : AbstractValidator<UpdateChecklistCommand>
    {
        public UpdateChecklistCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.UpdateChecklistModel).NotNull();
            RuleFor(command => command.UpdateChecklistModel.Title).NotEmpty();
		}
    }
}
