using System;

namespace Checkmunk.Domain.Common
{
    public interface IAmAuditable<out T>
    {
        DateTime CreatedAt { get; }

        T CreatedBy { get; }
    }

    public interface IAmAuditable : IAmAuditable<string>
    {
    }
}
