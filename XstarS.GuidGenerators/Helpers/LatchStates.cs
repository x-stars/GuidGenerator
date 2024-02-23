#if !UUIDREV_DISABLE
namespace XNetEx.Threading;

internal static class LatchStates
{
    public const int Initial = 0;
    public const int Entered = 1;
    public const int Exited = 2;
    public const int Failed = 3;
}
#endif
