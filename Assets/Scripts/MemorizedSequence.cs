using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[CreateAssetMenu(fileName = "Sequence", menuName = "ScriptableObjects/MemorizedSequenceOld", order = 1)]
public class MemorizedSequence : ScriptableObject
{
    public SequenceRow[] sequence = new SequenceRow[5];
    public int numberCellsInSequence = 16;

    public void WriteToSequence(SequenceRow[] sequenceFromTable)
    {
        for(int i = 0; i < 5; i++)
        {
            sequence[i] = new SequenceRow();
            sequence[i].rowEffect = sequenceFromTable[i].rowEffect;
            sequence[i].rowLevel = 1;
        }

        for(int s = 0; s< sequenceFromTable.Length;s++)
        {
            for(int i = 0; i < numberCellsInSequence; i++)
            {
                if (sequenceFromTable[s].GetStepFromRow(i))
                {
                    sequence[s].SetStepInRow(i, sequenceFromTable[s].GetStepFromRow(i));
                }
            }
        }
    }

    public SequenceRow[] GetSequenceFromMemory()
    {
        return sequence;
    }

    public void ClearMemorizedSequence()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int x = 0; x < numberCellsInSequence; x++)
            {
                sequence[i].RemoveStepFromRow(x);
            }
        }
    }

}

