using Checkmunk.Application.Users.Commands;
using FluentValidation;

namespace Checkmunk.Application.Users.CommandValidators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(command => command.EmailAddress).NotNull();
			RuleFor(command => command.UpdateUserModel.FirstName).NotNull();
			RuleFor(command => command.UpdateUserModel.LastName).NotNull();
		}
    }
}
