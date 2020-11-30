using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    [Header("Owner Data")]
    [HideInInspector] public Entity entity;

    [Header("Physics Variables")]
    public Rigidbody bulletRigidBody;
    public GravityBody gravityBody;

    [Header("Bullet Stats")]
    public int damage = 50;
    public float speed = 1.0f;
    public float lifeTime = 1.0f;

    [Header("Graphics Variables")]
    public GameObject explosionParticles;
    public float explosionLifeTime = 1f;

    //-----------------------------------------------------------------------------------//
    //Initialization and Update
    //-----------------------------------------------------------------------------------//
    public void Initialize(Entity entity, int damage, float speed, float lifeTime, Transform gunTransform)
    {
        this.entity = entity;
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;
        bulletRigidBody.velocity = gunTransform.forward * speed;
    }

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

    //-----------------------------------------------------------------------------------//
    //Change Planet Functions
    //-----------------------------------------------------------------------------------//
    private void OnTriggerEnter(Collider collider)
    {
        // If we hit something, destroy this object
        if (!collider.isTrigger)
        {
            // Ignore the player and player's current attractor body
            GravityAttractor gravityAttractor = collider?.gameObject?.GetComponent<GravityAttractor>();
            if (gravityAttractor != null)
            {
                GravityAttractor playerGravityAttractor = entity?.gameObject?.GetComponent<GravityBody>().gravityAttractor;
                if (gravityAttractor == playerGravityAttractor)
                {
                    return;
                }
            }

            Entity collidedEntity = collider?.gameObject?.GetComponent<Entity>();
            if (collidedEntity != null && collidedEntity.entityData.teamId == entity.entityData.teamId)
            {
                return;
            }
            else if (collidedEntity != null)
            {
                collidedEntity.DealDamage(damage);
            }

            GameObject explosion = Instantiate(explosionParticles, transform.position + (0.5f * gravityBody.gravityAttractor.gravityDirection), transform.rotation);
            Destroy(explosion, explosionLifeTime);
            Destroy(gameObject);      
        }
    }
    //-----------------------------------------------------------------------------------//
}
