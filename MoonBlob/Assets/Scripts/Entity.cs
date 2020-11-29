using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class Entity : MonoBehaviour
{
    [Header("Entity Data")]
    public EntityData entityData;
    public EntitySO entitySO;

    [Header("Input")]
    protected float x;
    protected float y;
    protected float mouseHorizontal;
    protected float mouseVertical;
    protected bool jumping;
    protected bool sprinting;
    protected bool dashLeft;
    protected bool dashRight;
    protected bool shoot;

    [Header("Physics Variables")]
    public Rigidbody entityRigidBody;
    public GravityBody gravityBody;

    [Header("Rotation Variables")]
    protected float xRotation;
    protected float sensitivity = 200f;

    [Header("Grounded Variables")]
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    protected bool grounded = true;

    [Header("Dash Variables")]
    protected bool readyToDash = true;

    [Header("Shooting Variables")]
    public GameObject gun;
    public GameObject shootOrigin;
    public GameObject bulletPrefab;

    [Header("Damage Variables")]
    [HideInInspector] public bool temporarilyInvinsible;

    [Header("Graphics Variables")]
    public List<MeshRenderer> meshRenderers;
    public Image healthBar;
    public TextMeshProUGUI healthText;

    //-----------------------------------------------------------------------------------//
    //Initialization and Update
    //-----------------------------------------------------------------------------------//
    public virtual void Start()
    {
        entityData = new EntityData(entitySO);

        // Create a new material for each mesh so that changes do not affect all entities with the material 
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            Material material = new Material(meshRenderer.sharedMaterial);
            meshRenderer.sharedMaterial = material;
        }
    }
    protected virtual void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);

        // Jumping and dashing in Update because they are single physics actions
        if (jumping)
        {
            Jump();
        }

        if (readyToDash && (dashLeft || dashRight)) 
        {
            Dash();
        }        

        if (gun != null)
        {
            RotateGun();
        }

        if (shoot)
        {
            Shoot();
        }
    }

    protected void FixedUpdate()
    {
        Move();
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Movement Functions
    //-----------------------------------------------------------------------------------//
    protected virtual void Move()
    {
        PlayerMovement();
    }

    protected virtual void PlayerMovement()
    {
        Vector3 direction = ((transform.forward * y) + (transform.right * x)).normalized;
        Vector3 velocity = direction * entityData.speed * Time.fixedDeltaTime;
        entityRigidBody.MovePosition(transform.position + velocity);
    }

    protected virtual void Jump()
    {
        if (!grounded) return;
        grounded = false;

        // Apply jump forces
        entityRigidBody.AddForce(transform.up * entityData.jumpForce * 1.5f);
        jumping = false;
    }

    protected virtual void Dash()
    {
        if (!readyToDash || (!dashLeft && !dashRight)) return;
        readyToDash = false;

        Vector3 dashVector = Vector3.zero;
        if (dashLeft)
        {
            dashVector = -transform.right * entityData.dashSpeed;
        }
        else if (dashRight)
        {
            dashVector = transform.right * entityData.dashSpeed;
        }

        Invoke(nameof(ResetDash), entityData.dashCooldown);
        entityRigidBody.MovePosition(transform.position + dashVector);
    }

    public void ResetDash()
    {
        readyToDash = true;
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Shooting Functions
    //-----------------------------------------------------------------------------------//
    protected virtual void RotateGun() { }

    protected virtual void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, shootOrigin.transform.position + (0.15f * shootOrigin.transform.forward), transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Initialize(this, entityData.bulletDamage, entityData.bulletSpeed, entityData.bulletLifeTime, gun.transform);
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Damage Functions
    //-----------------------------------------------------------------------------------//
    public virtual void DealDamage(int damage)
    {
        if (temporarilyInvinsible) return;

        entityData.health -= damage;
        if (entityData.health <= 0)
        {
            entityData.health = 0;
            Die();
        }

        if (entityData.invinsibleAfterDamage)
        {
            // Flicker Material Color after getting hit to indicate invinsibility
            bool applyBaseColor = false;
            Sequence sequence = DOTween.Sequence();
            float invinsibilityTime = entityData.invinsibliltyTime;
            float timeStep = 0.1f;
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                Color baseColor = meshRenderer.material.color;
                for (float i = 0.0f; i <= invinsibilityTime; i += timeStep)
                {
                    if (!applyBaseColor)
                    {
                        sequence.Insert(i, meshRenderer.material.DOColor(Color.white, timeStep)).SetEase(Ease.Linear);
                    }
                    else
                    {
                        sequence.Insert(i, meshRenderer.material.DOColor(baseColor, timeStep)).SetEase(Ease.Linear);
                    }
                    applyBaseColor = !applyBaseColor;
                }
            }

            temporarilyInvinsible = true;
            sequence.Play().OnComplete(ResetInvinsibility);
        }        
    }

    public void ResetInvinsibility()
    {
        temporarilyInvinsible = false;
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Death Functions
    //-----------------------------------------------------------------------------------//
    protected virtual void Die()
    {
        Destroy(this.gameObject);
    }
    //-----------------------------------------------------------------------------------//
}
