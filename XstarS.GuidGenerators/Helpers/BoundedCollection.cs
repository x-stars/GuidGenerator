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

[DebuggerDisplay($"Count = {{{nameof(Count)}}}")]
[DebuggerTypeProxy(typeof(BoundedCollection<>.DebugView))]
internal sealed class BoundedCollection<T>
    : IProducerConsumerCollection<T>, IReadOnlyCollection<T>
{
    private readonly ConcurrentQueue<T> Items;

    private volatile int ItemsCount;

    public BoundedCollection(int capacity)
    {
        this.Capacity = (capacity >= 0) ? capacity :
            throw new ArgumentOutOfRangeException(nameof(capacity));
        this.Items = new ConcurrentQueue<T>();
        this.ItemsCount = 0;
    }

    public int Count => this.Items.Count;

    public int Capacity { get; }

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


#if NETCOREAPP3_0_OR_GREATER
    public bool TryTake([MaybeNullWhen(false)] out T item)
#else
    public bool TryTake(out T item)
#endif
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

    private sealed class DebugView
    {
        private readonly BoundedCollection<T> Value;

        internal DebugView(BoundedCollection<T> value)
        {
            this.Value = value;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => this.Value.ToArray();
    }
}
