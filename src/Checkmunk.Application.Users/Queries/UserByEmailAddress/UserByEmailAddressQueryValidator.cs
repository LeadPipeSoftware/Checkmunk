using FluentValidation;

namespace Checkmunk.Application.Users.Queries.UserByEmailAddress
{
    public class UserByEmailAddressQueryValidator : AbstractValidator<UserByEmailAddressQuery>
    {
        public UserByEmailAddressQueryValidator()
        {
            DefaultValidatorExtensions.NotNull(RuleFor(query => query.EmailAddress));
        }
    }
}
