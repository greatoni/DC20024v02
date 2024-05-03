using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDamageSequences : SequencerUser
{
    public MemorizedRuneSong[] memorySlotSequence;
    public SequencerGlobal sequencerGlobal;
    public SequencerUser player; // for testing
    private bool doesWait = true;
    private int enemyFakeInputDelay;
    private int howManyTicksWait;
    private int tickTurnCountDown;

    public TextMeshProUGUI counterTurnsText;

    private void Start()
    {
        tickTurnCountDown = speedMax;
    }

    public void CustomUpdateFromMusicTick(string marker) //Turn based actions
    {
        counterTurnsText.text = tickTurnCountDown.ToString();

        if (doesWait)
        {
            tickTurnCountDown--;
            //Update progress bar for waiting

            if (tickTurnCountDown <= 0)
            {
                doesWait = false;
                tickTurnCountDown = howManyTicksInTurn;
                //OnTurnStart.Invoke();
                enemyFakeInputDelay = Random.Range(0, speedMax - 1);
                ReceiveStartTurn();
            }
        }
        if (!doesWait)
        {
            // Wait for players input or enemy automatic casting
            // if enemy random choice from memorizedSequences 

            tickTurnCountDown--;
                if (tickTurnCountDown == enemyFakeInputDelay)
                {
                    SendSequenceToGlobalSequencer(memorySlotSequence[0]);
                    tickTurnCountDown = speedMax;
                    doesWait = true;
                }
            

            if (tickTurnCountDown <= 0)
            {
                doesWait = true;
                tickTurnCountDown = speedMax;
            }
        }
    }


    public void ReceiveStartTurn()
    {
        // If damage only at turn start
        BurnState();
        FrozenState();
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
