using App.Metrics.Health;
using Checkmunk.Data.Contexts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Checkmunk.Application.Users.HealthChecks
{
    public class DatabaseHealthCheck : HealthCheck
    {
        private readonly CheckmunkContext context;

        public DatabaseHealthCheck(CheckmunkContext context)
            : base("DatabaseCheck")
        {
            this.context = context;
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(this.context.Users.Any()
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy());
        }
    }
}