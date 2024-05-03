using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerDamageSequences : SequencerUser
{

    public SequencerGlobal SequencerGlobal;
    public MemorizedRuneSong[] memorizedRuneSong;

    //BattleMenu battleMenu;

    public UnityEvent RecalculateStats; // later it should send integers of players stats to recalculate method on sequencerWEapon
    private bool doesWait = true;
    private int tickTurnCountDown;
    private int howManyTicksWait;
    public SequencerUser currentEnemy;

    public UnityEvent OnTurnStart, OnTurnEnd;

    private void Start()
    {

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
                OnTurnStart.Invoke();
                ReceiveStartTurn();
            }
        }
        if (!doesWait)
        {
            // Wait for players input or enemy automatic casting
            // if enemy random choice from memorizedSequences 

            tickTurnCountDown--;

            if (tickTurnCountDown <= 0)
            {
                doesWait = true;
                //RecalculateSpeed();
                tickTurnCountDown = howManyTicksWait;
            }
        }
    } 



    public void ReceiveStartTurn()
    {
        // If damage only at turn start
        BurnState();
        FrozenState();
    }

    public void ReceiveTick()
    {
        // Player receive damage at each tick or countable number of ticks
        //BurnState();
        //FrozenState
        //Erosion state
    }

    // method of calculation of chord damage



    public void SendSequenceToGlobalSequencer(MemorizedRuneSong sequence)
    {
        QueuedRunesong sequenceNew = new QueuedRunesong();
        sequenceNew = sequence.queueSequence;
        sequenceNew.isEnemy = false;
        sequenceNew.target = currentEnemy;
        sequenceNew.runesongStarter = this;
        for(int i = 0; i < 5; i++)
        {
            sequenceNew.parameterIndex[i] = fmodIndexes[i];
        }

        SequencerGlobal.AddNewSequenceToQueue(sequenceNew);
    }
    // Frozen state
    // Erosion state
}
