using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace GameOfLife.Common
{
    public class NotifyCollection<T> : IEnumerable<T>, INotifyCollectionChanged
    {
        private const int DefaultCcapacity = 4;
        private T[] array = new T[DefaultCcapacity];
        private int capacity = DefaultCcapacity;

        public int Count { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item)
        {
            EnsureCapacity();
            array[Count++] = item;
            RaiseCollectionChanged();
        }

        public T RemoveLast()
        {
            --Count;
            var last = array[Count];
            array[Count] = default(T);
            RaiseCollectionChanged();
            return last;
        }

        private void EnsureCapacity()
        {
            if (Count + 1 > capacity)
            {
                capacity *= 2;
                var newArray = new T[capacity];
                Array.Copy(array, newArray, array.Length);
                array = newArray;
            }
        }

        protected virtual void RaiseCollectionChanged()
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}