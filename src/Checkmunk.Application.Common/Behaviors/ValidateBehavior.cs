using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Checkmunk.Application.Common.Behaviors
{
    public class ValidateBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidateBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var failures = this.validators
                               .Select(validator => validator.Validate(request))
                               .SelectMany(result => result.Errors)
                               .Where(failure => failure != null)
                               .ToList();

            if (failures.Any())
            {
                throw new InvalidCommandOrQueryException($"Validation failed for type {typeof(TRequest).Name}", new ValidationException(failures));
            }

            return await next();
		}
	}
}
