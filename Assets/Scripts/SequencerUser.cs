using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequencerUser : MonoBehaviour
{
    public int healthMax = 1000;
    public int shieldMax = 200;
    public int speedMax = 6;
    public int howManyTicksInTurn;
    public int[] fmodIndexes = new int[5]; //this indexes used to make different characters to use different events inside fmod
    public int rowsDamageModifier = 0; // For imbalance

    int health, shield, speed;
    int burnTimes, burnAmount;
    int frozenTimes, frozenAmount;
    int regenerationTimes, regenerationAmount;

    public UnityEvent<SequencerUser> SendUser;

    public void SendUserOnClick() 
        //in Inspector if you link method(SequencerUser user) from any script to SendUser this method will get Sequencer user object
    {
        SendUser.Invoke(this);
    }

    public void OnNewGameStart()
    {
        if (gameManager.newGame)
        {
            health = healthMax;
            shield = shieldMax;
            speed = speedMax;
        }
    }

    public void SetMainCharacterStatsFromSave(
                                                int healthSaved, 
                                                int shieldSaved, 
                                                int speedSaved)
    {
        health = healthSaved;
        shield = shieldSaved;
        speed = speedSaved;
    }


    public int DamageHealth(int amount)
    {
        if (shield > 0)
        {
           amount = amount - DamageShield(amount);
        }

        shield = Mathf.Clamp(shield, 0, shieldMax);
        if (amount > 0)
        {
            health -= amount;
            health = Mathf.Clamp(health, 0, healthMax);
        }
        RefreshHUD(); // refresh health bar
        return health;
    }

    public int DamageShield(int amount)
    {
        shield -= amount;

        RefreshHUD(); // refresh shield it needs to clamp to zero in hud
        //shield = Mathf.Clamp(shield, 0, shieldMax);
        return shield;
    }

    public int ChangeSpeed(int amount)
    {
        speed += amount;
        speed = Mathf.Clamp(speed, 0, speedMax);
        return speed;
    }

    public int GetCurrentHealth()
    {
        return health;
    }
    public int GetCurrentSpeed()
    {
        return speed;
    }

    public void ChangeSequencerRowsDamage(int amount)
    {
        rowsDamageModifier += amount;
    }

    public void RefreshHUD()
    {

    }


    /// Status effects
    /// 
    /// There are burn, frozen, erosion, galvanisation, frostbite, imbalance states
    /// These states can be at the same time
    /// 
 
    ///
    /// Burn on start of the each turn 
    /// 

    public void Cleansing()
    {
        SetBurnState(0, 0);
        //UI update
    }


    public void SetBurnState(int amount, int times)
    {
        burnTimes = times;
        burnAmount = amount;
    }
    public void BurnState()
    {
        if (burnTimes > 0)
        {
            health -= burnAmount;
            burnTimes--;
            //UI update
            if (burnTimes <= 0)
            {
                //UI update turn off burn state
            }
        }
    }

    ///
    /// Frozen on start of the each turn 
    ///
    public void SetFrozenState(int amount, int times)
    {
        frozenTimes = times;
        frozenAmount = speed + amount;
    }
    public void FrozenState()
    {
        if (frozenTimes > 0)
        {
            speed = frozenAmount;
            frozenTimes--;

            //UI update
            if (frozenTimes <= 0)
            {
                speed = speedMax;
                //UI update turn off burn state
            }
        }
    }


    ///Regeneration state
    ///
    public void SetRegenerationState(int amount, int times)
    {
        regenerationTimes = times;
        regenerationAmount = amount;
    }
    public void RegenerationState()
    {
        if (regenerationTimes > 0)
        {
            health += regenerationAmount;
            regenerationTimes--;
            //UI update
            if (regenerationTimes <= 0)
            {
                //UI update turn off burn state
            }
        }
    }


    ///
    /// Erosion
    ///
}
