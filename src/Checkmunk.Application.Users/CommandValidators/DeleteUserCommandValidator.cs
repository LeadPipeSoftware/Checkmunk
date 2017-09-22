using Checkmunk.Application.Users.Commands;
using FluentValidation;

namespace Checkmunk.Application.Users.CommandValidators
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(command => command.EmailAddress).NotNull();
		}
    }
}
