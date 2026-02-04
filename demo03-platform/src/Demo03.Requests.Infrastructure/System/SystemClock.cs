using Demo03.Requests.Application.Abstractions;

namespace Demo03.Requests.Infrastructure.System
{
    public sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
