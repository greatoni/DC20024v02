using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageSequences : MonoBehaviour
{
    public int Speed = 5;
    public MemorizedSequence[] memorySlotSequence;
    public SequencerGlobal sequencerGlobal;
    private bool doesWait = true;
    private int tickTurnCountDown;
    private int enemyFakeInputDelay;
    public int howManyTicksInTurn;
    private int howManyTicksWait;

    private void Start()
    {
        howManyTicksWait = howManyTicksInTurn - Speed;


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
                    doesWait = true;
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
        sequencerGlobal.AddListenerToDetectBeat(this);

    }

    public void SendSequenceToGlobalSequencer(MemorizedSequence sequence)
    {
        QueueSequence sequenceNew = new QueueSequence();
        sequenceNew.sequence = sequence.sequence;
        sequenceNew.isEnemy = true;
        sequencerGlobal.AddNewSequenceToQueue(sequenceNew);
    }



}
