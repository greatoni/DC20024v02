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
        ClearMemorizedSequence();
        foreach (Elements i in System.Enum.GetValues(typeof(Elements)))
        {
            queueSequence.runesongPattern.Add(i, sequenceFromTable[i]);
        }
        foreach (Elements e in System.Enum.GetValues(typeof(Elements)))
        {
            queueSequence.occupiedRuneSlotsinRow.TryAdd(e, 0);
        }
        foreach (ChordTypes e in System.Enum.GetValues(typeof(ChordTypes)))
        {
            queueSequence.chordsPresent.TryAdd(e, 0);
        }
    }

    public QueuedRunesong GetSequenceFromMemory()
    {
        return queueSequence;
    }

    public void ClearMemorizedSequence()
    {
        if (queueSequence.runesongPattern.Count == 0) return;
        foreach (Elements i in System.Enum.GetValues(typeof(Elements)))
        {
            for (int x = 0; x < numberCellsInSequence; x++)
            {
                queueSequence.runesongPattern[i].RemoveStepFromRow(x);
            }
            queueSequence.occupiedRuneSlotsinRow[i] = 0;
        }
        foreach (ChordTypes e in System.Enum.GetValues(typeof(ChordTypes)))
        {
            queueSequence.chordsPresent[e] = 0 ;
        }
    }

    public void CountIntervals()
    {
        for (int i=0;i<numberCellsInSequence;i++)
        {
            List<Elements> tempElements = new List<Elements>();
            foreach (Elements e in System.Enum.GetValues(typeof(Elements)))
            {
                if (queueSequence.runesongPattern[e].GetStepFromRow(i))
                {
                    tempElements.Add(e);
                    queueSequence.occupiedRuneSlotsinRow[e] = queueSequence.occupiedRuneSlotsinRow[e] + 1;
                    //Debug.Log("runes in pattern " + queueSequence.occupiedRuneSlotsinRow[e]); 
                    //we can count rune charge if it enough
                }
            }
            
            if (tempElements.Contains(Elements.Fire) && tempElements.Contains(Elements.Ice))
            {
                queueSequence.chordsPresent[ChordTypes.FireIce] = queueSequence.chordsPresent[ChordTypes.FireIce] + 1;
            }
            if (tempElements.Contains(Elements.Fire) && tempElements.Contains(Elements.Earth))
            {
                queueSequence.chordsPresent[ChordTypes.FireEarth] = queueSequence.chordsPresent[ChordTypes.FireEarth] + 1;
            }
            if (tempElements.Contains(Elements.Fire) && tempElements.Contains(Elements.Air))
            {
                queueSequence.chordsPresent[ChordTypes.FireAir] = queueSequence.chordsPresent[ChordTypes.FireAir] + 1;
            }

            if (tempElements.Contains(Elements.Ice) && tempElements.Contains(Elements.Earth))
            {
                queueSequence.chordsPresent[ChordTypes.IceEarth] = queueSequence.chordsPresent[ChordTypes.IceEarth] + 1;
            }
            if (tempElements.Contains(Elements.Ice) && tempElements.Contains(Elements.Air))
            {
                queueSequence.chordsPresent[ChordTypes.IceAir] = queueSequence.chordsPresent[ChordTypes.IceAir] + 1;
            }
            if (tempElements.Contains(Elements.Air) && tempElements.Contains(Elements.Earth))
            {
                queueSequence.chordsPresent[ChordTypes.EarthAir] = queueSequence.chordsPresent[ChordTypes.EarthAir] + 1;
            }
        }
        //Debug.Log( "Ice + Air count "+queueSequence.chordsPresent[ChordTypes.IceAir]);
    }


}

