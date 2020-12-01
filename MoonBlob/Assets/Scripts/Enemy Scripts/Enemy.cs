using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Energy Variables")]
    protected float shootElapsedTime = 0.0f;

    //-----------------------------------------------------------------------------------//
    //Enemy Initialization and Update
    //-----------------------------------------------------------------------------------//
    public override void Initialize()
    {
        base.Initialize(); 
        healthBar.fillAmount = (float)entityData.health / (float)entitySO.health;
        healthText.text = String.Format("{0} / {1}", (int)entityData.health, (int)entitySO.health);

        int moonId = -1;
        if (gravityBody?.gravityAttractor)
        {
            moonId = gravityBody.gravityAttractor.moonId;
        }
        GameManager.instance.AddWaveUI(this, moonId);

        // Given enemies some initial movement
        y = 1;
    }

    protected override void Update()
    {
        GetEnemyInput();
        base.Update();
    }

    protected virtual void GetEnemyInput()
    {
        // Move only if playerController is on the same Planet as you
        if (GameManager.instance.localPlayer.gravityBody.gravityAttractor == gravityBody?.gravityAttractor)
        {
            transform.LookAt(GameManager.instance.localPlayer.transform, gravityBody.gravityAttractor.gravityDirection);
            y = 1;
        }

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
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Damage Functions
    //-----------------------------------------------------------------------------------//
    public override void DealDamage(int damage)
    {
        base.DealDamage(damage);
        healthBar.fillAmount = (float)entityData.health / (float)entitySO.health;
        healthText.text = String.Format("{0} / {1}", (int)entityData.health, (int)entitySO.health);
    }
    //-----------------------------------------------------------------------------------//
    
    //-----------------------------------------------------------------------------------//
    //Collision Functions
    //-----------------------------------------------------------------------------------//
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.isTrigger) return;

        PlayerController playerController = collision?.collider.gameObject.GetComponent<PlayerController>();
        if (playerController != null && !playerController.temporarilyInvinsible)
        {
            playerController.DealDamage(entityData.touchDamage);

            // Apply a force to the player controller and flicker its material
            Vector3 collideDirection = collision.impulse.normalized;
            playerController.entityRigidBody.AddForce(5 * collideDirection, ForceMode.Impulse);
        }
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Death Functions
    //-----------------------------------------------------------------------------------//
    protected override void Die()
    {
        GameManager.instance.RemoveWaveUI(this);
        GameManager.instance.currentWave.RemoveActiveEnemy(entityData.entityId);
        base.Die();
    }
    //-----------------------------------------------------------------------------------//

}
