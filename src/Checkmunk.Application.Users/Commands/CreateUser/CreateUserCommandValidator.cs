using FluentValidation;

namespace Checkmunk.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            DefaultValidatorExtensions.NotNull(RuleFor(command => command.CreateUserModel.EmailAddress));
			DefaultValidatorExtensions.NotNull(RuleFor(command => command.CreateUserModel.FirstName));
			DefaultValidatorExtensions.NotNull(RuleFor(command => command.CreateUserModel.LastName));
		}
    }
}
