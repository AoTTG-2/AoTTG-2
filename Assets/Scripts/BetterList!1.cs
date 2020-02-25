using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BetterList<T>
{
    public T[] buffer;
    public int size;

    public void Add(T item)
    {
        if ((this.buffer == null) || (this.size == this.buffer.Length))
        {
            this.AllocateMore();
        }
        this.buffer[this.size++] = item;
    }

    private void AllocateMore()
    {
        T[] array = (this.buffer == null) ? new T[0x20] : new T[Mathf.Max(this.buffer.Length << 1, 0x20)];
        if ((this.buffer != null) && (this.size > 0))
        {
            this.buffer.CopyTo(array, 0);
        }
        this.buffer = array;
    }

    public void Clear()
    {
        this.size = 0;
    }

    public bool Contains(T item)
    {
        if (this.buffer != null)
        {
            for (int i = 0; i < this.size; i++)
            {
                if (this.buffer[i].Equals(item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    [DebuggerHidden]
    public IEnumerator<T> GetEnumerator()
    {
        return new GetEnumeratorc__Iterator9 { f__this = (BetterList<T>)this };
    }

    public void Insert(int index, T item)
    {
        if ((this.buffer == null) || (this.size == this.buffer.Length))
        {
            this.AllocateMore();
        }
        if (index < this.size)
        {
            for (int i = this.size; i > index; i--)
            {
                this.buffer[i] = this.buffer[i - 1];
            }
            this.buffer[index] = item;
            this.size++;
        }
        else
        {
            this.Add(item);
        }
    }

    public T Pop()
    {
        if ((this.buffer != null) && (this.size != 0))
        {
            T local = this.buffer[--this.size];
            this.buffer[this.size] = default(T);
            return local;
        }
        return default(T);
    }

    public void Release()
    {
        this.size = 0;
        this.buffer = null;
    }

    public bool Remove(T item)
    {
        if (this.buffer != null)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < this.size; i++)
            {
                if (comparer.Equals(this.buffer[i], item))
                {
                    this.size--;
                    this.buffer[i] = default(T);
                    for (int j = i; j < this.size; j++)
                    {
                        this.buffer[j] = this.buffer[j + 1];
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        if ((this.buffer != null) && (index < this.size))
        {
            this.size--;
            this.buffer[index] = default(T);
            for (int i = index; i < this.size; i++)
            {
                this.buffer[i] = this.buffer[i + 1];
            }
        }
    }

    public void Sort(Comparison<T> comparer)
    {
        bool flag = true;
        while (flag)
        {
            flag = false;
            for (int i = 1; i < this.size; i++)
            {
                if (comparer(this.buffer[i - 1], this.buffer[i]) > 0)
                {
                    T local = this.buffer[i];
                    this.buffer[i] = this.buffer[i - 1];
                    this.buffer[i - 1] = local;
                    flag = true;
                }
            }
        }
    }

    public T[] ToArray()
    {
        this.Trim();
        return this.buffer;
    }

    private void Trim()
    {
        if (this.size > 0)
        {
            if (this.size < this.buffer.Length)
            {
                T[] localArray = new T[this.size];
                for (int i = 0; i < this.size; i++)
                {
                    localArray[i] = this.buffer[i];
                }
                this.buffer = localArray;
            }
        }
        else
        {
            this.buffer = null;
        }
    }

    public T this[int i]
    {
        get
        {
            return this.buffer[i];
        }
        set
        {
            this.buffer[i] = value;
        }
    }

    [CompilerGenerated]
    private sealed class GetEnumeratorc__Iterator9 : IEnumerator, IDisposable, IEnumerator<T>
    {
        internal T Scurrent;
        internal int SPC;
        internal BetterList<T> f__this;
        internal int i__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    if (this.f__this.buffer == null)
                    {
                        goto Label_0086;
                    }
                    this.i__0 = 0;
                    break;

                case 1:
                    this.i__0++;
                    break;

                default:
                    goto Label_008D;
            }
            if (this.i__0 < this.f__this.size)
            {
                this.Scurrent = this.f__this.buffer[this.i__0];
                this.SPC = 1;
                return true;
            }
        Label_0086:
            this.SPC = -1;
        Label_008D:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        T IEnumerator<T>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }
}

