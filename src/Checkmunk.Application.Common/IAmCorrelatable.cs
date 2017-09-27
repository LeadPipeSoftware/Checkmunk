using System;

namespace Checkmunk.Application.Common
{
    public interface IAmCorrelatable
    {
        Guid CorrelationGuid { get; }
    }
}