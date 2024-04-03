using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequencerUser : MonoBehaviour
{
    public int health = 1000;
    public int shield = 200;
    public int speed = 6;
    public int howManyTicksInTurn;
    public int[] fmodIndexes = new int[5];

    public UnityEvent<SequencerUser> SendUser;

    public void SendUserOnClick()
    {
        SendUser.Invoke(this);
    }

}
