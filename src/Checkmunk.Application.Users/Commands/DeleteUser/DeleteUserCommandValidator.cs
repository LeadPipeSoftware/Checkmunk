using FluentValidation;

namespace Checkmunk.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            DefaultValidatorExtensions.NotNull(RuleFor(command => command.EmailAddress));
		}
    }
}
