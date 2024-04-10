using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlotsCreator : MonoBehaviour
{

    /// <summary>
    /// This class is for generating memory slots for enemies 
    /// Scriptable objects used here placed on enemies sequencerWeapon scripts
    /// </summary>
    public MemorizedRuneSong[] acolytes, spirits; // Should be the same as enemy sequences
    public EnemySequence[] acolytesBool, spiritsBool;// Should be the same as enemy MemorizedSequence
    public int[] baseRowDamage = new int[5];
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < acolytes.Length; i++)
        {
            acolytes[i].WriteToSequence(ConvertEnemySequenceToSequence(acolytesBool[i]).runesongPattern);
            acolytes[i].CountIntervals();
        }
        for (int i = 0; i < spirits.Length; i++)
        {
            spirits[i].WriteToSequence(ConvertEnemySequenceToSequence(spiritsBool[i]).runesongPattern);
            spirits[i].CountIntervals();
        }
    }

    QueuedRunesong ConvertEnemySequenceToSequence(EnemySequence boolSequences)
    {

        QueuedRunesong newSequencer = new QueuedRunesong();
        int count = 0;

        foreach (Elements i in System.Enum.GetValues(typeof(Elements)))
        {
            newSequencer.runesongPattern[i] = new SequenceRow();
            newSequencer.runesongPattern[i].rowBaseDamage = baseRowDamage[count];
            count++;
        }

        for (int i = 0; i < boolSequences.Neutral.Length; i++)
        {
            if (boolSequences.Neutral[i])
            {
                newSequencer.runesongPattern[Elements.Neutral].SetStepInRow(i*2, true);
            }
        }
        for (int i = 0; i < boolSequences.Fire.Length; i++)
        {
            if (boolSequences.Fire[i])
            {
                newSequencer.runesongPattern[Elements.Fire].SetStepInRow(i * 2, true);
            }
        }
        for (int i = 0; i < boolSequences.Ice.Length; i++)
        {
            if (boolSequences.Ice[i])
            {
                newSequencer.runesongPattern[Elements.Ice].SetStepInRow(i * 2, true);
            }
        }
        for (int i = 0; i < boolSequences.Earth.Length; i++)
        {
            if (boolSequences.Earth[i])
            {
                newSequencer.runesongPattern[Elements.Earth].SetStepInRow(i * 2, true);
            }
        }
        for (int i = 0; i < boolSequences.Air.Length; i++)
        {
            if (boolSequences.Air[i])
            {
                newSequencer.runesongPattern[Elements.Air].SetStepInRow(i * 2, true);
            }
        }
        //print(newSequencer.runesongPattern[Elements.Neutral].GetDictioneryCount().ToString());
        return newSequencer;
    }
}

[System.Serializable]
public class EnemySequence
{
    public bool[] Neutral = new bool[8];
    public bool[] Fire = new bool[8];
    public bool[] Ice = new bool[8];
    public bool[] Earth = new bool[8];
    public bool[] Air = new bool[8];
}
