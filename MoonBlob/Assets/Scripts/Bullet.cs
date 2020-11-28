using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    //Need Some stat scriptable object here


    public Rigidbody bulletRigidBody;

    public GravityBody gravityBody;

    // The Player that shot the bullet (will be entity soon)
    public PlayerController playerController;

    [Header("Bullet Stats")]
    public int speed;
    public float lifeTime = 1f;

    [Header("Graphics Variables")]
    public GameObject explosionParticles;
    public float explosionLifeTime = 1f;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
        {
            GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation);
            Destroy(explosion, explosionLifeTime);
            Destroy(gameObject);
        }
    }

    //-----------------------------------------------------------------------------------//
    //Change Planet Functions
    //-----------------------------------------------------------------------------------//
    private void OnTriggerEnter(Collider collider)
    {
        // If we hit something, destroy this object
        if (!collider.isTrigger)
        {
            // Ignore the player and player's current attractor body
            GravityAttractor gravityAttractor = collider.gameObject.GetComponent<GravityAttractor>();
            if (gravityAttractor != null)
            {
                GravityAttractor playerGravityAttractor = playerController.gameObject.GetComponent<GravityBody>().gravityAttractor;
                if (gravityAttractor == playerGravityAttractor)
                {
                    return;
                }
            }

            PlayerController collidedPlayerController = collider.gameObject.GetComponent<PlayerController>();
            if (collidedPlayerController != null)
            {
                return;
            }

            GameObject explosion = Instantiate(explosionParticles, transform.position + (0.5f * gravityBody.gravityAttractor.gravityDirection), transform.rotation);
            Destroy(explosion, explosionLifeTime);
            Destroy(gameObject);      
        }
    }
    //-----------------------------------------------------------------------------------//
}
