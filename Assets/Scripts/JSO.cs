using System;
using System.Collections.Generic;
using UnityEngine;
using HUD;

namespace JSO {
    public class IndexJSO<int,T>
    {
        int key = get; set;
        T value = get; set;
        public IndexJSO(int key, T value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public class StringJSO<string,T>
    {
        string key = get; set;
        T value = get; set;
        public StringJSO(string key, T value)
        {
            this.key = key;
            this.value = value;
        }
    }
    
    public class CharJSO<char,T>
    {
        char key = get; set;
        T value = get; set;
        public CharJSO(char key, T value)
        {
            this.key = key;
            this.value = value;
        }
    }
    
    public class SequenceTypeJSO<SequenceType,T>
    {
        SequenceType key = get; set;
        T value = get; set;
        public SequenceTypeJSO(SequenceType key, T value)
        {
            this.key = key;
            this.value = value;
        }
    }
}


