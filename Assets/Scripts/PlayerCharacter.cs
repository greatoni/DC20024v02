using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour
{
    public int HPMax;
    public int armourMax;
    int armourCurrent;
    int HP; // I need an accessor method for the HUD to query the current HP. I wrote one and included it, but if you would prefer to do something else, let me know -@jade
    int turnSpeed; //Step to start action
    public int runeMaxCapacity;
    int runeCapacity;
    int continousDamageTimes;
    /// <summary>
    /// Possible player can have an invetory 
    /// </summary>

    public Vector3 lastSpawnCoordinates;

    public UnityEvent DeathEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RuneQuantityChange(int amount)
    {
        runeCapacity += amount;
        runeCapacity = Mathf.Clamp(runeCapacity, 0, runeMaxCapacity);
        UpdateUI();
    }

    public void DamagePlayerHP(int amount)
    {
        HP -= amount;
        HP = Mathf.Clamp(HP, 0, HPMax);
        UpdateUI();
        if (HP == 0)
        {
            DeathEvent.Invoke();
        }
    }

    public void DamagePlayerSpeed(int amount)
    {
        turnSpeed += amount;
        UpdateUI();
    }

    public void LaunchContinousDamage(int times)
    {
        continousDamageTimes = times;
    }

    public void ContinousDamage(int amount)
    {
        if (continousDamageTimes > 0)
        {
            HP -= amount;
            HP = Mathf.Clamp(HP, 0, HPMax);
            UpdateUI();
            if (HP == 0)
            {
                DeathEvent.Invoke();
            }
        }
    }


    public void UpdateUI()
    {
        //Update heals, speed, sequencer, rune capacity, shield(armour)
    }

    public void DeathEventHappens()
    {
        runeCapacity = (int)((float)runeMaxCapacity * 0.6f);
    }

    public void RefillAllValues()
    {
        HP = HPMax;
        runeCapacity = runeMaxCapacity;
    }

    public int GetHP()
    {
        return HP;
    }
}


public enum States
{
    Frozen,
    Burn,
    Erosion,
    Galvanization,
    Imbalance,
    Frostbite

}