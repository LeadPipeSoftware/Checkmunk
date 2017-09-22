using Checkmunk.Application.Users.Commands;
using FluentValidation;

namespace Checkmunk.Application.Users.CommandValidators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(command => command.CreateUserModel.EmailAddress).NotNull();
			RuleFor(command => command.CreateUserModel.FirstName).NotNull();
			RuleFor(command => command.CreateUserModel.LastName).NotNull();
		}
    }
}
