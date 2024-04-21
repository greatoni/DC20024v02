using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerDamageSequences : MonoBehaviour
{
    public int healthMax = 1000;
    public int shieldMax = 200;
    public int speedMax = 5;
    public int rowsDamageModifier = 0; // For imbalance
    public SequencerGlobal SequencerGlobal;

    public MemorizedSequence[] memorizedRuneSong;

    int burn = 0, burnAmount = 0, frozen = 0, frozenAmount = 0, erosion = 0;
    int currentHealth, currentShield, speed;
    //BattleMenu battleMenu;

    public UnityEvent RecalculateStats; // later it should send integers of players stats to recalculate method on sequencerWEapon
    private bool doesWait = true;
    private int tickTurnCountDown;
    private int howManyTicksWait;
    public int howManyTicksInTurn = 32;

    public UnityEvent OnTurnStart, OnTurnEnd;

    private void Start()
    {
        currentHealth = healthMax;

    }
    public void CustomUpdateFromMusicTick(string marker) //Turn based actions
    {
        if (doesWait)
        {
            tickTurnCountDown--;


            //Update progress bar for waiting
            //battleMenu.ChangePlayerProgressBar(tickTurnCountDown, howManyTicksWait, false);

            if (tickTurnCountDown <= 0)
            {
                doesWait = false;
                //RecalculateSpeed();
                tickTurnCountDown = howManyTicksInTurn;
                OnTurnStart.Invoke();
            }
        }
        if (!doesWait)
        {
            // Wait for players input or enemy automatic casting
            // if enemy random choice from memorizedSequences 

            tickTurnCountDown--;

                //battleMenu.ChangePlayerProgressBar(tickTurnCountDown, howManyTicksInTurn, true);

            if (tickTurnCountDown <= 0)
            {
                doesWait = true;
                //RecalculateSpeed();
                tickTurnCountDown = howManyTicksWait;
            }
        }
    } 

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public int GetCurrentSpeed()
    {
        return speed;
    }
    public void PlayerHealthDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, healthMax);
    }

    public void PlayerShieldDamage(int amount)
    {
        currentShield -= amount;
        currentShield = Mathf.Clamp(currentShield, 0, shieldMax);
    }
    public void ReduceSpeed(int amount)
    {
        speed -= amount;
        speed = Mathf.Clamp(currentShield, 0, speedMax);
    }
    public void SequencerRowsDamage(int amount)
    {
        rowsDamageModifier = amount;
    }

    public void ReceiveStartTurn()
    {
        // If damage only at turn start
        BurnState();
    }

    public void ReceiveTick()
    {
        // Player receive damage at each tick or countable number of ticks
        //BurnState();
        //FrozenState
        //Erosion state
    }

    // method of calculation of chord damage
    public void SetBurnState(int amount, int times)
    {
        burn = times;
        burnAmount = amount;
    }

    void BurnState()
    {
        if (burn > 0)
        {
            healthMax -= burnAmount;
            burn--;
            //UI update
            if (burn <= 0)
            {
                //UI update turn off burn state
            }
        }
    }
    public void SendSequenceToGlobalSequencer(MemorizedSequence sequence)
    {
        QueueSequence sequenceNew = new QueueSequence();
        sequenceNew.sequence = sequence.sequence;
        sequenceNew.isEnemy = false;
        SequencerGlobal.AddNewSequenceToQueue(sequenceNew);
    }
    // Frozen state
    // Erosion state
}
