using System.Collections;
using System.Collections.Generic;
using UnityEngine;using System.Linq;
using FMOD.Studio;

public class SequencerGlobal : MonoBehaviour
{
    public DetectBeat detectBeat;
    public FMODUnity.EventReference[] exploreMusic;

    Queue<QueueSequence> queueSequences = new Queue<QueueSequence>();
    Dictionary<QueueSequence, int> queueCounters = new Dictionary<QueueSequence, int>();
    List<Elements> elementInColumnPlayer = new List<Elements>();
    List<Elements> elementInColumnEnemy = new List<Elements>();
    //If Queue is not empty Step sequencer read though all sequences in a queue each tick but for each of them needs to be
    // its own counter
    public FMODUnity.EventReference[] battlemusic;
    private EventInstance exploreMusicInstance;

    private void Start()
    {
        exploreMusicInstance = FMODUnity.RuntimeManager.CreateInstance(exploreMusic[0]);
        exploreMusicInstance.start();
        exploreMusicInstance.setParameterByName("EndMusic", 0);
    }


    public void StartBattle()
    {
        detectBeat.StartBattleMusic(battlemusic[Random.Range(0, battlemusic.Length - 1)]);
        exploreMusicInstance.setParameterByName("EndMusic", 1);
    }

    public void StepSequencer()
    {
        foreach(Elements e in elementInColumnPlayer)
        {
            print("player does damage");
        }
        foreach (Elements e in elementInColumnEnemy)
        {
            print("enemy does damage");
        }

        elementInColumnPlayer.Clear();
        elementInColumnEnemy.Clear();
        List<QueueSequence> tempQueue = new List<QueueSequence>();
        Queue<QueueSequence> queueTemp = new Queue<QueueSequence>();

        foreach (QueueSequence q in queueSequences)
        {
            queueTemp.Enqueue(q);
        }
        foreach (QueueSequence q in queueTemp)
        {
            tempQueue.Add(queueSequences.Dequeue());
        }
        queueTemp.Clear();

        foreach (QueueSequence q in tempQueue)
        {
            
            if (queueCounters[q] < 16)
            {
                //play sequence at queueCounters[fromQueue] if it even digit 
                for (int i = 0; i < 5; i++)
                {
                    if (q.sequence[i].GetStepFromRow(queueCounters[q]))
                    {
                        if(!q.isEnemy) elementInColumnPlayer.Add((Elements)System.Enum.ToObject(typeof(Elements), i));
                        if (q.isEnemy) elementInColumnEnemy.Add((Elements)System.Enum.ToObject(typeof(Elements), i));
                    }
                }

                if (elementInColumnPlayer.Count == 0 && elementInColumnEnemy.Count == 0)
                {
                    detectBeat.SetParameterToInstance("SequenceStep01", 0);
                    detectBeat.SetParameterToInstance("SequenceStep02", 0);
                    detectBeat.SetParameterToInstance("SequenceStep03", 0);
                    detectBeat.SetParameterToInstance("SequenceStep04", 0);
                    detectBeat.SetParameterToInstance("SequenceStep05", 0);
                }
                // It possible to compare two lists and cancel out element if enemy and player cast the same thing
                foreach (Elements e in elementInColumnPlayer)
                {
                    string nameParameter = "";
                    switch (e)
                    {
                        case Elements.Neutral:
                            nameParameter = "SequenceStep01";
                            break;
                        case Elements.Fire:
                            nameParameter = "SequenceStep02";
                            break;
                        case Elements.Ice:
                            nameParameter = "SequenceStep03";
                            break;
                        case Elements.Earth:
                            nameParameter = "SequenceStep04";
                            break;
                        case Elements.Air:
                            nameParameter = "SequenceStep05";
                            break;
                    }
                    detectBeat.SetParameterToInstance(nameParameter, 1);
                }
                foreach (Elements e in elementInColumnEnemy)
                {
                    string nameParameter = "";
                    switch (e)
                    {
                        case Elements.Neutral:
                            nameParameter = "SequenceStep01";
                            break;
                        case Elements.Fire:
                            nameParameter = "SequenceStep02";
                            break;
                        case Elements.Ice:
                            nameParameter = "SequenceStep03";
                            break;
                        case Elements.Earth:
                            nameParameter = "SequenceStep04";
                            break;
                        case Elements.Air:
                            nameParameter = "SequenceStep05";
                            break;
                    }
                    detectBeat.SetParameterToInstance(nameParameter, 1);
                }
                //print(queueCounters[q]);
                queueCounters[q] = queueCounters[q]+1;
                queueSequences.Enqueue(q);
            }
            else
            {
                queueCounters.Remove(q);
            }
        }
    }


    void PlayQueueItem(int index, QueueSequence queueMember)
    {

    }

    void FinishBattle(EnemyDamageSequences enemy)
    {
        detectBeat.OnMarkerPress.RemoveListener(enemy.CustomUpdateFromMusicTick);
    }

    public void AddListenerToDetectBeat(EnemyDamageSequences enemy)
    {
        detectBeat.OnMarkerPress.AddListener(enemy.CustomUpdateFromMusicTick);
    }

    public void AddNewSequenceToQueue(QueueSequence queueMember)
    {
        queueSequences.Enqueue(queueMember);
        queueCounters.Add(queueMember, 0);
    }
}

public class QueueSequence
{
    public SequenceRow[] sequence = new SequenceRow[5];
    public bool isEnemy = true;
}


[System.Serializable]
public enum ChordTypes
{
    FireIce,
    FireEarth,
    FireAir,
    IceEarth,
    IceAir,
    EarthAir,
    None
}

[System.Serializable]
public enum Elements
{
    Neutral,
    Fire,
    Ice,
    Earth,
    Air
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


[System.Serializable]
public class SequenceRow
{
    Dictionary<int, bool> Row = new Dictionary<int, bool>();
    public Elements rowEffect;
    public int rowLevel = 1;
    public int rowBaseDamage = 10;
    public void SetStepInRow(int index, bool isOccupied)
    {
        if (Row.TryAdd(index, isOccupied))
        { //Debug.Log("added successfully");
        }
    }

    public bool GetStepFromRow(int index)
    {
        if (Row.TryGetValue(index, out bool isOccupied))
        {
            return isOccupied;
        }
        return false;
    }

    public void RemoveStepFromRow(int index)
    {
        Row.Remove(index);
    }

}