using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using FMOD.Studio;

public class SequencerGlobal : MonoBehaviour
{
    public DetectBeat detectBeat;
    public FMODUnity.EventReference[] exploreMusic;

    Queue<QueuedRunesong> queueSequences = new Queue<QueuedRunesong>();
    Dictionary<QueuedRunesong, int> queueCounters = new Dictionary<QueuedRunesong, int>();
    List<QueuedRunesong> savedSongsForNextStep = new List<QueuedRunesong>();

    HashSet<SequencerUser> battleParticipants = new HashSet<SequencerUser>();
    Dictionary<QueuedRunesong, List<Elements>> hitList = new Dictionary<QueuedRunesong, List<Elements>>();
    public FMODUnity.EventReference[] battlemusic, sfx, combo;
    private EventInstance exploreMusicInstance, elementSfx, comboSFX;
    //int evenCounter = 0;
    bool battleMusicOn = false;
    public TextMeshProUGUI message, comboMessage;
    public float turnDelayTime = 1;

    public UnityEvent counterTurnEvent;

    private void Start()
    {
        message.text = "";
        StartExplorationMusic(exploreMusic[0]);
    }

    public void StartExplorationMusic(FMODUnity.EventReference musicExplore)
    {
        exploreMusicInstance = FMODUnity.RuntimeManager.CreateInstance(musicExplore);
        exploreMusicInstance.setParameterByName("EndMusic", 0);
        exploreMusicInstance.start();
    }

    public void PlayElement(FMODUnity.EventReference sfxElement)
    {
        elementSfx = FMODUnity.RuntimeManager.CreateInstance(sfxElement);
        elementSfx.start();
    }
    public void PlayCombo(FMODUnity.EventReference sfxElement)
    {
        comboSFX = FMODUnity.RuntimeManager.CreateInstance(sfxElement);
        comboSFX.start();
    }

    public void StartBattle()
    {
        if (!battleMusicOn)
        {
            print("start battle music");
            detectBeat.StartBattleMusic(battlemusic[Random.Range(0, battlemusic.Length - 1)]);
            exploreMusicInstance.setParameterByName("EndMusic", 1);
            
            battleMusicOn = true;
            StartCoroutine(CounterTurns());
        }
    }

    public bool CheckifQueueStillThere(QueuedRunesong qrunesong)
    {

        return queueSequences.Contains(qrunesong);
    }

    public void StepSequencer()
    {
        
        //Check for enemy and players enemies
        foreach (QueuedRunesong r in savedSongsForNextStep)
        {
/*            if (hitList[r].Contains(Elements.Air) && hitList[r].Contains(Elements.Ice))
            {
                //PlayCombo(combo[0]);
               
                StartCoroutine(PrintTextCombo(Elements.Air, Elements.Ice));
            }*/
            foreach (Elements e in hitList[r])
            {
                /*                if (r.isEnemy) { print("player damaged"); }
                                else { print("enemy damaged"); }
                                print(e + " damage " + r.target);*/
                ////
                ////This switch statement is alternative to play sfx from fmod 
                ///

               /* switch (e)
                {
                    case Elements.Neutral:
                        PlayElement(sfx[0]);
                        break;
                    case Elements.Fire:
                        PlayElement(sfx[1]);
                        break;
                    case Elements.Ice:
                        PlayElement(sfx[2]);
                        break;
                    case Elements.Earth:
                        PlayElement(sfx[3]);
                        break;
                    case Elements.Air:
                        PlayElement(sfx[4]);
                        break;
                }*/

                StartCoroutine(PrintText(e, r));
            }
            hitList[r].Clear();
        }

        savedSongsForNextStep.Clear();
        List<QueuedRunesong> tempQueue = new List<QueuedRunesong>();
        Queue<QueuedRunesong> queueTemp = new Queue<QueuedRunesong>();

        foreach (QueuedRunesong q in queueSequences) //duplicate queue to Dequeue original
        {
            queueTemp.Enqueue(q);
        }

        foreach (QueuedRunesong q in queueTemp)
        {
            tempQueue.Add(queueSequences.Dequeue());
        }
        queueTemp.Clear();

        foreach (QueuedRunesong q in tempQueue)
        {

            if (!queueCounters.TryGetValue(q, out int tempCount)) return;

            if (queueCounters[q] < 16)
            {
                
                //play sequence at queueCounters[fromQueue] if it even digit 
                foreach (Elements i in System.Enum.GetValues(typeof(Elements)))
                {
                    if (q.runesongPattern[i].GetStepFromRow(queueCounters[q]))
                    {
                        hitList[q].Add(i);
                    }
                }


                if (hitList[q].Count == 0)
                {
                        detectBeat.SetParameterToInstance(("SequenceStep0" + q.parameterIndex[0].ToString()), 0);
                        detectBeat.SetParameterToInstance(("SequenceStep0" + q.parameterIndex[1].ToString()), 0);
                        detectBeat.SetParameterToInstance(("SequenceStep0" + q.parameterIndex[2].ToString()), 0);
                        detectBeat.SetParameterToInstance(("SequenceStep0" + q.parameterIndex[3].ToString()), 0);
                        detectBeat.SetParameterToInstance(("SequenceStep0" + q.parameterIndex[4].ToString()), 0); 
                }
                else
                {
                    savedSongsForNextStep.Add(q);

                    foreach (Elements e in hitList[q])
                    {
                        string nameParameter = "";
                        switch (e)
                        {
                            case Elements.Neutral:
                                nameParameter = ("SequenceStep0" + q.parameterIndex[0].ToString());
                                break;
                            case Elements.Fire:
                                nameParameter = ("SequenceStep0" + q.parameterIndex[1].ToString());
                                break;
                            case Elements.Ice:
                                nameParameter = ("SequenceStep0" + q.parameterIndex[2].ToString());
                                break;
                            case Elements.Earth:
                                nameParameter = ("SequenceStep0" + q.parameterIndex[3].ToString());
                                break;
                            case Elements.Air:
                                nameParameter = ("SequenceStep0" + q.parameterIndex[4].ToString());
                                break;
                        }
                        detectBeat.SetParameterToInstance(nameParameter, 1);
                        //Debug.Log("Parameter " + nameParameter);
                    }                                                                               
                }
                queueCounters[q] = queueCounters[q]+1;
                queueSequences.Enqueue(q);
            }
            else
            {
                print("Sequence finished");
                //Count possibility of bonus damage
                OnFinishRuneSong(q);
                queueCounters.Remove(q);
            }
        }

/*        evenCounter++;
        if(evenCounter == 10000) { evenCounter = 0; }*/
    }

    void OnFinishRuneSong(QueuedRunesong q)
    {

        /// Count for target Burn State and Cleansing
        /// 
        if (q.occupiedRuneSlotsinRow[Elements.Fire] > 0)
        {
            int fireRand = Random.Range(0, 100);
            if (fireRand < q.occupiedRuneSlotsinRow[Elements.Fire] * 10)
            {
                print("BURN !!! " + q.target);
                q.target.SetBurnState(q.occupiedRuneSlotsinRow[Elements.Fire] * (int)(q.runesongPattern[Elements.Fire].rowBaseDamage * 0.5f), 5);
            }
            fireRand = Random.Range(0, 100);
            if (fireRand < q.occupiedRuneSlotsinRow[Elements.Fire] * 10)
            {
                q.runesongStarter.Cleansing();
            }
        }

        ///
        /// Ice States 
        /// 

        if (q.occupiedRuneSlotsinRow[Elements.Ice] > 0)
        {
            q.target.SetFrozenState(q.occupiedRuneSlotsinRow[Elements.Ice] * 2, 5);
            q.runesongStarter.SetRegenerationState(q.occupiedRuneSlotsinRow[Elements.Ice]*5, 5); //one point for rune is small number
        }

    }

    void FinishBattle(EnemyDamageSequences enemy)
    {
        detectBeat.OnMarkerPress.RemoveListener(enemy.CustomUpdateFromMusicTick);
        StopCoroutine(CounterTurns());
    }


    public void AddListenerToDetectBeat(EnemyDamageSequences enemy)
    {
        if (!battleMusicOn)
        {
            print("add listener");
            detectBeat.OnMarkerPress.AddListener(enemy.CustomUpdateFromMusicTick);
        }

    }

    public void AddNewSequenceToQueue(QueuedRunesong queueMember)
    {
        
        if(!queueCounters.TryAdd(queueMember, 0)){
            if (!queueSequences.Contains(queueMember))
            {
                queueCounters.Remove(queueMember);
                queueCounters.TryAdd(queueMember, 0);
                hitList.Remove(queueMember);
                hitList.TryAdd(queueMember, new List<Elements>());
            }
        }
        else
        {
           hitList.TryAdd(queueMember, new List<Elements>());
        }

            queueSequences.Enqueue(queueMember);

        
    }


    IEnumerator PrintText(Elements e, QueuedRunesong r)
    {
        message.text = e + " damaged " + r.target;
        yield return new WaitForSeconds(1);
        message.text = "";

    }
    IEnumerator PrintTextCombo(Elements A, Elements B)
    {
        comboMessage.text = "Combo " +A+" - "+ B;
        yield return new WaitForSeconds(1);
        comboMessage.text = "";

    }

    IEnumerator CounterTurns()
    {
        while (battleMusicOn)
        {
            counterTurnEvent.Invoke();
            yield return new WaitForSeconds(turnDelayTime);
        }

    }

    private void OnDestroy()
    {
        counterTurnEvent.RemoveAllListeners();
    }

}

public class QueuedRunesong
{
    public Dictionary<Elements, SequenceRow> runesongPattern = new Dictionary<Elements, SequenceRow>();
    public bool isEnemy = true;
    public SequencerUser target;
    public SequencerUser runesongStarter;
    public Dictionary<ChordTypes, int> chordsPresent = new Dictionary<ChordTypes, int>();
    public Dictionary<Elements, int> occupiedRuneSlotsinRow = new Dictionary<Elements, int>();
    public int[] parameterIndex = new int[5];

    public void MakeParameterIndexes(int endNumber)
    { 
        for(int i=0;i < 5; i++)
        {
            parameterIndex[i] = endNumber-5+i+1;
            
        }
    }
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
    None //enum can't be null
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

    public int GetDictionaryCount()
    {
        foreach(int  i in Row.Keys)
        {
            //Debug.Log(i + " fff");
        }
        return Row.Count;
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