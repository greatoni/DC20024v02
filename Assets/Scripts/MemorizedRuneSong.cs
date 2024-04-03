using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[CreateAssetMenu(fileName = "Sequence", menuName = "ScriptableObjects/MemorizedSequence", order = 1)]
public class MemorizedRuneSong : ScriptableObject
{
    public QueuedRunesong queueSequence = new QueuedRunesong();
    public int numberCellsInSequence = 16;

    public void WriteToSequence(Dictionary<Elements,SequenceRow> sequenceFromTable)
    {
        foreach (Elements i in System.Enum.GetValues(typeof(Elements)))
        {
            queueSequence.runesongPattern.Add(i, sequenceFromTable[i]);
        }
    }

    public QueuedRunesong GetSequenceFromMemory()
    {
        return queueSequence;
    }

    public void ClearMemorizedSequence()
    {
        foreach (Elements i in System.Enum.GetValues(typeof(Elements)))
        {
            for (int x = 0; x < numberCellsInSequence; x++)
            {
                queueSequence.runesongPattern[i].RemoveStepFromRow(x);
            }
        }
    }

}

