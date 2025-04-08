using System;

namespace XNetEx;

internal sealed class DisposeAction(Action<bool> action)
    : IDisposable
{
    private readonly Action<bool> Action = action ??
        throw new ArgumentNullException(nameof(action));

    private bool IsDisposed = false;

    ~DisposeAction()
    {
        this.Dispose(disposing: false);
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!this.IsDisposed)
        {
            this.Action.Invoke(disposing);
            this.IsDisposed = true;
        }
    }
}
