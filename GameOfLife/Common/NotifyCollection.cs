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
    private int count;

   
    public int Count
    {
      get { return count; }
    }

    public void Add(T item)
    {
      EnsureCapacity();
      array[count++] = item;
      RaiseCollectionChanged();
    }

    public T RemoveLast()
    {
      --count;
      var last = array[count];
      array[count] = default(T);
      RaiseCollectionChanged();
      return last;
    }

    public IEnumerator<T> GetEnumerator()
    {
      for (int i = 0; i < count; i++)
        yield return array[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private void EnsureCapacity()
    {
      if (count + 1 > capacity)
      {
        capacity *= 2;
        var newArray = new T[capacity];
        Array.Copy(array, newArray, array.Length);
        array = newArray;
      }
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    protected virtual void RaiseCollectionChanged()
    {
      var handler = CollectionChanged;
      if (handler != null) 
        handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    
  }
}