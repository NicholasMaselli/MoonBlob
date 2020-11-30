using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blurp : Enemy
{
    protected override void Die()
    {
        // Spawn 2 Peeps on death
        GameManager.instance.dataDB.enemyPrefabs.TryGetValue("Peep", out GameObject peepPrefrab);
        if (peepPrefrab != null)
        {
            int peepCount = 2;
            for (int i = 0; i < peepCount; i++)
            {
                GameObject peepGO = Instantiate(peepPrefrab, transform.position, transform.rotation, GameManager.instance.entityMap);
                Enemy enemy = peepGO.GetComponent<Enemy>();
                enemy.Initialize();

                GameManager.instance.currentWave.AddActiveEnemy(enemy);
            }
        }
        base.Die();
    }
}
