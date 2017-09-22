using Checkmunk.Application.Users.Queries;
using FluentValidation;

namespace Checkmunk.Application.Users.QueryValidators
{
    public class UserByEmailAddressQueryValidator : AbstractValidator<UserByEmailAddressQuery>
    {
        public UserByEmailAddressQueryValidator()
        {
            RuleFor(query => query.EmailAddress).NotNull();
        }
    }
}
