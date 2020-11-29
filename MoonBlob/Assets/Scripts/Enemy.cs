using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //-----------------------------------------------------------------------------------//
    //Enemy Initialization and Update
    //-----------------------------------------------------------------------------------//
    protected override void Update()
    {
        GetEnemyInput();
        base.Update();
    }

    private void GetEnemyInput()
    {
        // Move only if playerController is on the same Planet as you
        if (GameManager.instance.localPlayer.gravityBody.gravityAttractor == gravityBody.gravityAttractor)
        {
            transform.LookAt(GameManager.instance.localPlayer.transform, gravityBody.gravityAttractor.gravityDirection);
            y = 1;


            /*
            if (GameManager.instance.localPlayer.transform.position.x > transform.position.x)
            {
                x = 1;
            }
            else if (GameManager.instance.localPlayer.transform.position.x > transform.position.x)
            {
                x = -1;
            }
            else
            {
                x = 0;
            }

            if (GameManager.instance.localPlayer.transform.position.y > transform.position.y)
            {
                y = 1;
            }
            else if (GameManager.instance.localPlayer.transform.position.y > transform.position.y)
            {
                y = -1;
            }
            else
            {
                y = 0;
            }
            */
        }
        else
        {
            x = 0;
        }
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

            Vector3 collideDirection = collision.impulse.normalized;

            // Apply a force to the player controller and flicker its material
            playerController.entityRigidBody.AddForce(5 * collideDirection, ForceMode.Impulse);
        }
    }
    //-----------------------------------------------------------------------------------//

}
