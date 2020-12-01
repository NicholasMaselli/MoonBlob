using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
    // Place this collider inside each planet and outside the ring of the edge of space.
    // If an entity enters the moon collider or exists the space collider, move it to a
    // random spawn point
    public bool moonCollider = true;

    //-----------------------------------------------------------------------------------//
    //Resawpn Collider Functions
    //-----------------------------------------------------------------------------------//
    private void OnTriggerEnter(Collider collider)
    {
        if (moonCollider)
        {
            CheckRespawn(collider);
        }
        
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!moonCollider)
        {
            CheckRespawn(collider);
        }        
    }

    public void CheckRespawn(Collider collider)
    {
        if (collider.isTrigger) return;

        Entity entity = collider.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            // Move the entity to a random spawn point
            int largestPowerOfTwo = WaveSystem.FindLargestPowerOfTwoLessThanOrEqualToNumber(GameManager.instance.currentWave.waveNumber + 1);
            int moonCount = largestPowerOfTwo + 1;
            Vector3 spawnPoint = GameManager.instance.GetRandomSpawn(moonCount);
            entity.transform.position = spawnPoint;
        }
    }
    //-----------------------------------------------------------------------------------//
}
