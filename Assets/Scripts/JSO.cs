using System;
using System.Collections.Generic;
using HUD;


namespace JavascriptStyleObject
{
    public class JSO<TKey, TValue>
    {
        private List<TKey> keys = new List<TKey>();
        private List<TValue> values = new List<TValue>();

        private JSO(TKey key, TValue value)
        {
            keys.Add(key);
            values.Add(value);
        }

        public void Add(TKey key, TValue value)
        {
            keys.Add(key);
            values.Add(value);
        }

        public TValue Get(TKey key)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].Equals(key))
                {
                    return values[i];
                }
            }
            throw new KeyNotFoundException($"Key {key} not found.");
        }

        public void Remove(TKey key)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].Equals(key))
                {
                    keys.RemoveAt(i);
                    values.RemoveAt(i);
                    return;
                }
            }
            throw new KeyNotFoundException($"Key {key} not found.");
        }

        public class IndexJSO<TValue> : JSO<int, TValue>
        {
            public IndexJSO(int key, TValue value) : base(key, value)
            {
            }
        }

        public class StringJSO<T> : JSO<string, T>
        {
            public StringJSO(string key, T value) : base(key, value)
            {
            }
        }

        public class CharJSO<T> : JSO<char, T>
        {
            public CharJSO(char key, T value) : base(key, value)
            {
            }
        }

        public class SequenceTypeJSO<T> : JSO<SequenceType, T>
        {
            public SequenceTypeJSO(SequenceType key, T value) : base(key, value)
            {
            }
        }
    }
}




