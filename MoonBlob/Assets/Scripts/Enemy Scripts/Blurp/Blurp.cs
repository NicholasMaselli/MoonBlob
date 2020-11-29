using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blurp : Enemy
{
    //-----------------------------------------------------------------------------------//
    //Death Functions
    //-----------------------------------------------------------------------------------//
    protected override void Die()
    {
        // Spawn 2 Peeps on death
        GameManager.instance.dataDB.enemyPrefabs.TryGetValue("Peep", out GameObject peepPrefrab);
        if (peepPrefrab != null)
        {
            int peepCount = 2;
            for (int i = 0; i < peepCount; i++)
            {
                Instantiate(peepPrefrab, transform.position, transform.rotation);
            }
        }
        Destroy(this.gameObject);
    }
    //-----------------------------------------------------------------------------------//
}
