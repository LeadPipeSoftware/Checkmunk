using FluentValidation;

namespace Checkmunk.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            DefaultValidatorExtensions.NotNull(RuleFor(command => command.EmailAddress));
			DefaultValidatorExtensions.NotNull(RuleFor(command => command.UpdateUserModel.FirstName));
			DefaultValidatorExtensions.NotNull(RuleFor(command => command.UpdateUserModel.LastName));
		}
    }
}
