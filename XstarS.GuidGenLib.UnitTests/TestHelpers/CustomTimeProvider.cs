#if NET8_0_OR_GREATER
using System;

namespace XNetEx;

internal sealed class CustomTimeProvider(Func<DateTimeOffset> timestampProvider)
    : TimeProvider()
{
    public override DateTimeOffset GetUtcNow() => timestampProvider.Invoke();
}
#endif
