using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityData
{
    [Header("Entity Stats")]
    public int health = 100;
    public int energy = 10;
    public float speed = 5;

    [Header("Jump Stats")]
    public float jumpForce = 500;

    [Header("Dash Stats")]
    public float dashCooldown = 1;
    public float dashSpeed = 5;

    [Header("Touch Damage")]
    public int touchDamage = 5;

    [Header("Invinsible After Damage")]
    public bool invinsibleAfterDamage = false;
    public float invinsibliltyTime = 2.0f;

    public EntityData() { }
    public EntityData(EntitySO entitySO)
    {
        this.health = entitySO.health;
        this.energy = entitySO.energy;
        this.speed = entitySO.speed;

        this.jumpForce = entitySO.jumpForce;

        this.dashCooldown = entitySO.dashCooldown;
        this.dashSpeed = entitySO.dashSpeed;

        this.touchDamage = entitySO.touchDamage;

        this.invinsibleAfterDamage = entitySO.invinsibleAfterDamage;
        this.invinsibliltyTime = entitySO.invinsibliltyTime;
    }
}
