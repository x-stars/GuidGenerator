#if !UUIDREV_DISABLE
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
#if NETCOREAPP3_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace XNetEx.Collections.Concurrent;

[DebuggerDisplay(
    $"{nameof(this.Count)} = {{{nameof(this.Count)}}}, " +
    $"{nameof(this.Capacity)} = {{{nameof(this.Capacity)}}}")]
[DebuggerTypeProxy(typeof(BoundedCollection<>.DebugView))]
internal sealed class BoundedCollection<T>(int capacity)
    : IProducerConsumerCollection<T>, IReadOnlyCollection<T>
{
    private readonly ConcurrentQueue<T> Items = new();

    private volatile int ItemsCount = 0;

    public int Count => this.Items.Count;

    public int Capacity { get; } = (capacity >= 0) ? capacity :
        throw new ArgumentOutOfRangeException(
            nameof(capacity), "Value must be non-negative.");

    object ICollection.SyncRoot => ((ICollection)this.Items).SyncRoot;

    bool ICollection.IsSynchronized => ((ICollection)this.Items).IsSynchronized;

    public void CopyTo(T[] array, int index) => this.Items.CopyTo(array, index);

    public IEnumerator<T> GetEnumerator() => this.Items.GetEnumerator();

    public T[] ToArray() => this.Items.ToArray();

    public bool TryAdd(T item)
    {
        if (Interlocked.Increment(ref this.ItemsCount) <= this.Capacity)
        {
            this.Items.Enqueue(item);
            return true;
        }
        Interlocked.Decrement(ref this.ItemsCount);
        return false;
    }

    public bool TryTake(
#if NETCOREAPP3_0_OR_GREATER
        [MaybeNullWhen(false)]
#endif
        out T item)
    {
        if (this.Items.TryDequeue(out item))
        {
            Interlocked.Decrement(ref this.ItemsCount);
            return true;
        }
        return false;
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ((ICollection)this.Items).CopyTo(array, index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this.Items).GetEnumerator();
    }

    private sealed class DebugView(BoundedCollection<T> value)
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => value.ToArray();
    }
}
#endif
