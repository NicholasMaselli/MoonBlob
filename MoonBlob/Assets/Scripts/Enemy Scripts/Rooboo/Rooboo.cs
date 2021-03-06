﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooboo : Enemy
{
    protected override void GetEnemyInput()
    {
        transform.LookAt(GameManager.instance.localPlayer.transform, GameManager.instance.localPlayer.transform.up);
        
        shoot = false;
        if (gun != null)
        {
            shootElapsedTime += Time.deltaTime;
            if (shootElapsedTime > entityData.shootTime)
            {
                shoot = true;
                shootElapsedTime = 0.0f;
            }
        }
    }

    protected override void PlayerMovement()
    {
        Vector3 direction = (GameManager.instance.localPlayer.transform.position - transform.position).normalized;
        Vector3 velocity = direction * entityData.speed;

        entityRigidBody.velocity = velocity;
    }
}
