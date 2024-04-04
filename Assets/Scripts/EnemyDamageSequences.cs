using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageSequences : SequencerUser
{
    public MemorizedRuneSong[] memorySlotSequence;
    public SequencerGlobal sequencerGlobal;
    public SequencerUser player; // for testing
    private bool doesWait = true;
    private int enemyFakeInputDelay;
    private int howManyTicksWait;
    private int tickTurnCountDown;

    private void Start()
    {
        howManyTicksWait = howManyTicksInTurn - speedMax;
    }

    public void CustomUpdateFromMusicTick(string marker) //Turn based actions
    {
        if (doesWait)
        {
            tickTurnCountDown--;
            //Update progress bar for waiting
            if (tickTurnCountDown <= 0)
            {
                doesWait = false;
                //RecalculateSpeed();
                tickTurnCountDown = howManyTicksInTurn;
                //OnTurnStart.Invoke();
                enemyFakeInputDelay = Random.Range(0, howManyTicksWait - 1);
            }
        }
        if (!doesWait)
        {
            // Wait for players input or enemy automatic casting
            // if enemy random choice from memorizedSequences 

            tickTurnCountDown--;

                //print("send pattern to global");
                if (tickTurnCountDown == enemyFakeInputDelay)
                {
                    SendSequenceToGlobalSequencer(memorySlotSequence[0]);
                    //RecalculateSpeed();
                    tickTurnCountDown = howManyTicksWait;
                    doesWait = true;
                }
            

            if (tickTurnCountDown <= 0)
            {
                doesWait = true;
                //RecalculateSpeed();
                tickTurnCountDown = howManyTicksWait;
            }
        }
    }

    public void SendEnemyToGlobal()
    {
        //sequencerGlobal.AddListenerToDetectBeat(this);
    }

    public void SendSequenceToGlobalSequencer(MemorizedRuneSong sequence)
    {
        
        QueuedRunesong sequenceNew = new QueuedRunesong();
        sequenceNew = sequence.queueSequence;
        sequenceNew.isEnemy = true;
        sequenceNew.target = player;
        sequenceNew.runesongStarter = this;
        for (int i = 0; i < 5; i++)
        {
            sequenceNew.parameterIndex[i] = fmodIndexes[i];
        }
        if (!sequencerGlobal.CheckifQueueStillThere(sequenceNew))
        {
            sequencerGlobal.AddNewSequenceToQueue(sequenceNew);
        }
            
    }



}
