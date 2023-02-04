using System;

namespace XNetEx;

internal sealed class DisposeAction : IDisposable
{
    private readonly Action<bool> Action;

    private bool IsDisposed;

    public DisposeAction(Action<bool> action)
    {
        this.Action = action ??
            throw new ArgumentNullException(nameof(action));
        this.IsDisposed = false;
    }

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
