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
    public bool heatSeeking = false;

    [Header("Graphics Variables")]
    public GameObject explosionParticles;
    public float explosionLifeTime = 1f;

    //-----------------------------------------------------------------------------------//
    //Initialization and Update
    //-----------------------------------------------------------------------------------//
    public void Initialize(Entity entity, int damage, float speed, float lifeTime, Transform gunTransform, bool heatSeeking)
    {
        this.entity = entity;
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;
        bulletRigidBody.velocity = gunTransform.forward * speed;
    }

    private void Update()
    {
        if (GameManager.instance.gameEnded) return;
        if (GameManager.instance.paused) return;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
        {
            GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation);
            Destroy(explosion, explosionLifeTime);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (heatSeeking == false) return;

        transform.LookAt(GameManager.instance.localPlayer.transform, GameManager.instance.localPlayer.transform.up);

        Vector3 direction = (GameManager.instance.localPlayer.transform.position - transform.position).normalized;
        Vector3 velocity = direction * speed;

        bulletRigidBody.velocity = velocity;
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Bullet Trigger Functions
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
                GravityAttractor playerGravityAttractor = entity?.gravityBody?.gravityAttractor;
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

            if (transform != null && gravityBody?.gravityAttractor != null)
            {
                GameObject explosion = Instantiate(explosionParticles, transform.position + (0.5f * gravityBody.gravityAttractor.gravityDirection), transform.rotation);
                Destroy(explosion, explosionLifeTime);
            }           
            
            Destroy(gameObject);      
        }
    }
    //-----------------------------------------------------------------------------------//
}
