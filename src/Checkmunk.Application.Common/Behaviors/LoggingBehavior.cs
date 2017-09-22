using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkmunk.Application.Common.Behaviors
{
	public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<TRequest> logger;

        public LoggingBehavior(ILogger<TRequest> logger)
		{
			this.logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
		{
            logger.LogInformation($"Handling the {typeof(TRequest).Name} request");

			var response = await next();

            logger.LogInformation($"Handled the {typeof(TRequest).Name} request");

			return response;
		}
	}
}
