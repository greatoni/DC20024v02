using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SequencerUser : MonoBehaviour
{
    public int healthMax = 1000;
    public int shieldMax = 200;
    public int speedMax = 6;
    public int howManyTicksInTurn;
    public int[] fmodIndexes = new int[5]; //this indexes used to make different characters to use different events inside fmod
    public int rowsDamageModifier = 0; // For imbalance
    public int shieldModifier = 0;

    int health, shield, speed;
    int burnTimes, burnAmount;
    int frozenTimes, frozenAmount;
    int regenerationTimes, regenerationAmount;
    int regenerationShieldTimes, regenerationShieldAmount;
    int galvinTimes, galvinAmount;

    public TextMeshPro healthBar;

    public HashSet<SequencerUserStates> characterStatesList = new HashSet<SequencerUserStates>();

    public UnityEvent<SequencerUser> SendUser;

    public UnityEvent death;



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

    public void SetMainCharacterStatsFromSave(  int healthSaved, 
                                                int shieldSaved, 
                                                int speedSaved)
    {
        health = healthSaved;
        shield = shieldSaved;
        speed = speedSaved;
    }


    public int DamageHealth(int amount)
    {

        if (amount > 0)
        {
            int passShield = shield - amount;
            
            if (passShield < 0)
            {
                health += passShield;
                health = Mathf.Clamp(health, 0, healthMax);
            }
        }
        if (health <= 0)
        {
            death.Invoke();
        }
        RefreshHUD(); // refresh health bar
        return health;
    }

    public int DamageShield(int amount)
    {
        shield -= amount;
        RefreshHUD(); // refresh shield it needs to clamp to zero in hud
        shield = Mathf.Clamp(shield, 0, shieldMax + shieldModifier);
        return shield;
    }

    public void SetShieldModifier(int modifier)
    {
        shieldModifier = modifier;
        shield += shieldModifier;
        print("shield now"+shield);
        RefreshHUD(); // Maybe it's good to use events for it
    }

    public int ChangeSpeed(int amount)
    {
        speed += amount;
        speed = Mathf.Clamp(speed, 0, speedMax);
        return speed;
    }

    public int GetCurrentHealth(out int max)
    {
        max = healthMax;
        return health;
    }
    public int GetCurrentSpeed(out int max)
    {
        max = speedMax;
        return speed;
    }

    public int GetDamageWithModifier(QueuedRunesong q, Elements e)
    {
        print("damage modifier" + rowsDamageModifier);
        if (rowsDamageModifier > q.runesongPattern[e].rowBaseDamage) return 0;
        return q.runesongPattern[e].rowBaseDamage + rowsDamageModifier;
    }

    public void ChangeSequencerRowsDamage(int amount)
    {
        rowsDamageModifier += amount; //Galvanization and imbalance 
    }

    public void RefreshHUD()
    {

        if (health > 0 && healthBar!=null) { healthBar.text = health.ToString(); }
        
    }


    public int ReturnStatesPoints(SequencerUserStates state)
    {
        switch (state)
        {
            case SequencerUserStates.Burn:
                return burnAmount;
                
            case SequencerUserStates.Frozen:
                return frozenAmount;
                
            case SequencerUserStates.Regeneration:
                break;
            case SequencerUserStates.Erosion:
                return shieldModifier;
                
            case SequencerUserStates.Imbalance:
                break;
            case SequencerUserStates.Galvinization:
                break;
        }
        return 0;
    }

    /// Status effects
    /// 
    /// There are burn, frozen, erosion, galvanisation, frostbite, imbalance states
    /// These states can be at the same time
    /// 

    ///
    /// Burn on start of the each turn 
    /// 

    public void Cleansing(SequencerUserStates state)
    {
        switch (state)
        {
            case SequencerUserStates.Burn:
                SetBurnState(0, 0);
                break;
            case SequencerUserStates.Frozen:
                SetFrozenState(0, 0);
                break;
            case SequencerUserStates.Erosion:
                shield = shieldMax+ shieldModifier;
                break;
            case SequencerUserStates.Imbalance:
                rowsDamageModifier = 0;
                break;
        }
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
        print("frozen " + amount);
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
            health = Mathf.Clamp(health, 0, healthMax);
            regenerationTimes--;
            //UI update
            if (regenerationTimes <= 0)
            {
                //UI update turn off burn state
            }
        }
    }

    ///Regeneration state
    ///
    public void SetRegenerationShieldState(int amount, int times)
    {
        regenerationShieldTimes = times;
        regenerationShieldAmount = amount;
    }
    public void RegenerationShieldState()
    {
        if (regenerationShieldTimes > 0)
        {
            shield += regenerationShieldAmount;
            shield = Mathf.Clamp(health, 0, shieldMax+shieldModifier);
            regenerationShieldTimes--;
            //UI update
            if (regenerationShieldTimes <= 0)
            {
                //UI update turn off burn state
            }
        }
    }

    ///
    /// Galvanization state
    ///
    public void SetGalvinizationState(int amount, int times)
    {
        //galvinTimes = times;
        //galvinAmount = amount;
    }
    public void GalvinizationState()
    {
        if (galvinTimes > 0)
        {
            rowsDamageModifier = galvinAmount;
            galvinTimes--;
            //UI update
            if (galvinTimes <= 0)
            {
                rowsDamageModifier = 0;
                //UI update turn off burn state
            }
        }
    }
}


