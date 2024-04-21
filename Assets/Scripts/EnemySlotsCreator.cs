using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlotsCreator : MonoBehaviour
{

    /// <summary>
    /// This class is for generating memory slots for enemies 
    /// Scriptable objects used here placed on enemies sequencerWeapon scripts
    /// </summary>
    public MemorizedSequence[] acolytes, spirits; // Should be the same as enemy sequences
    public EnemySequence[] acolytesBool, spiritsBool;// Should be the same as enemy MemorizedSequence
    public int[] baseRowDamage = new int[5];
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < acolytes.Length; i++)
        {
            acolytes[i].WriteToSequence(ConvertEnemySequenceToSequence(acolytesBool[i]));
        }
        for (int i = 0; i < spirits.Length; i++)
        {
            spirits[i].WriteToSequence(ConvertEnemySequenceToSequence(spiritsBool[i]));
        }
    }

    SequenceRow[] ConvertEnemySequenceToSequence(EnemySequence boolSequences)
    {

        SequenceRow[] newSequencer = new SequenceRow[5];
        for(int i = 0; i < newSequencer.Length; i++)
        {
            newSequencer[i] = new SequenceRow();
            newSequencer[i].rowBaseDamage = baseRowDamage[i];
        }

        for (int i = 0; i < boolSequences.Neutral.Length; i++)
        {
            if (boolSequences.Neutral[i])
            {
                newSequencer[0].SetStepInRow(i*2, true);
            }
        }
        for (int i = 0; i < boolSequences.Fire.Length; i++)
        {
            if (boolSequences.Fire[i])
            {
                newSequencer[1].SetStepInRow(i * 2, true);
            }
        }
        for (int i = 0; i < boolSequences.Ice.Length; i++)
        {
            if (boolSequences.Ice[i])
            {
                newSequencer[2].SetStepInRow(i * 2, true);
            }
        }
        for (int i = 0; i < boolSequences.Earth.Length; i++)
        {
            if (boolSequences.Earth[i])
            {
                newSequencer[3].SetStepInRow(i * 2, true);
            }
        }
        for (int i = 0; i < boolSequences.Air.Length; i++)
        {
            if (boolSequences.Air[i])
            {
                newSequencer[4].SetStepInRow(i * 2, true);
            }
        }

        return newSequencer;

    }



}
