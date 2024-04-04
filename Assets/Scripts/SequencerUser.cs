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
    public int[] fmodIndexes = new int[5];
    public int rowsDamageModifier = 0; // For imbalance

    int health, shield, speed;

    public UnityEvent<SequencerUser> SendUser;

    public void SendUserOnClick() 
        //in Inspector if you link method(SequencerUser user) from any script to SendUser this method will get Sequencer user object
    {
        SendUser.Invoke(this);
    }

    public void OnGameStart()
    {
        if (gameManager.newGame)
        {
            health = healthMax;
            shield = shieldMax;
            speed = speedMax;
            
        }
    }

    public int ChangeHealth(int amount)
    {
        return health + amount;
    }

    public int ChangeShield(int amount)
    {
        return shield + amount;
    }

    public int ChangeSpeed(int amount)
    {
        return speed + amount;
    }
    public int GetCurrentHealth()
    {
        return health;
    }
    public int GetCurrentSpeed()
    {
        return speedMax;
    }

    public void SequencerRowsDamage(int amount)
    {
        rowsDamageModifier = amount;
    }
}
