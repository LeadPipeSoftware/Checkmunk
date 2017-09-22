using System.Threading.Tasks;
using MediatR;
using App.Metrics;
using App.Metrics.Core.Options;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace Checkmunk.Application.Common.Behaviors
{
	public class TimerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
        private readonly IMetrics metrics;

        public TimerBehavior(IMetrics metrics)
        {
            this.metrics = metrics;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var requestTimer = new TimerOptions
            {
                Name = $"{typeof(TRequest).Name}",
                MeasurementUnit = App.Metrics.Unit.Requests,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds
            };

            using(metrics.Measure.Timer.Time(requestTimer))
            {
                var response = await next();

                return response;
            }
		}
	}
}
