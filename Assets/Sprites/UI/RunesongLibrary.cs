using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine;

public enum SequenceType {
    Neutral,
    Fire,
    Ice,
    Earth,
    Air
}

public class Sequence : Dictionary<int,bool> {
    public SequenceType type;
    private readonly SequenceType _type = SequenceType.Neutral;
        
    public Sequence(SequenceType type) : base(8)
    {
        this.type = type;
    }
}

public class Runesong : Dictionary<string,Sequence> {
    private readonly string alias = "";
    private Sequence neutral = new(SequenceType.Neutral);
    private Sequence fire = new(SequenceType.Fire);
    private Sequence ice = new(SequenceType.Ice);
    private Sequence earth = new(SequenceType.Earth);
    private Sequence air = new(SequenceType.Air);

    public Runesong(string alias) {
        this.alias = alias;
    }
    public Runesong(string alias, Sequence neutral, Sequence fire, Sequence ice, Sequence earth, Sequence air) {
        this.alias = alias;
        this.neutral = neutral;
        this.fire = fire;
        this.ice = ice;
        this.earth = earth;
        this.air = air;
    }

    public string GetAlias(){
        return this.alias;
    }

    public Sequence GetSequence(SequenceType type){
        if(type == SequenceType.Neutral) return neutral;
        else if(type == SequenceType.Fire) return fire;
        else if(type == SequenceType.Ice) return ice;
        else if(type == SequenceType.Earth) return earth;
        else if(type == SequenceType.Air) return air;
        else throw new Exception("Invalid sequence type");
    }

    public void SetSequence(SequenceType type, Sequence sequence){
        if(type == SequenceType.Neutral) this.neutral = sequence;
        else if(type == SequenceType.Fire) this.fire = sequence;
        else if(type == SequenceType.Ice) this.ice = sequence;
        else if(type == SequenceType.Earth) this.earth = sequence;
        else if(type == SequenceType.Air) this.air = sequence;
        else throw new Exception("Invalid sequence type");
    }
}

public class RunesongLibrary : MonoBehaviour
{
    private readonly Dictionary<string,Runesong> library = new();
    private readonly int selectedIndex;

    public RunesongLibrary(){
        return;
    }

    public RunesongLibrary(Runesong runesong){
        library.Add(runesong.GetAlias(), runesong);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}


