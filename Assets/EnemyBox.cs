using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour
{

    public List<EnemyDamageSequences> enemies = new List<EnemyDamageSequences>();

    public void EnemiesToSequencerGlobal()
    {
        foreach(EnemyDamageSequences e in enemies)
        {
            e.SendEnemyToGlobal();
        }
    }


}
