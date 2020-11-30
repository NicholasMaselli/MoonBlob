using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityData
{
    [Header("Entity Identity")]
    public int entityId;
    public string entityName;
    public int teamId;

    [Header("Entity Stats")]
    public int health = 100;
    public float speed = 5;
    public float sprintSpeed = 8;

    [Header("Jump Stats")]
    public float jumpForce = 500;

    [Header("Dash Stats")]
    public float dashCooldown = 1;
    public float dashSpeed = 5;

    [Header("Shoot Stats")]
    public int bulletDamage = 50;
    public float bulletSpeed = 1.0f;
    public float bulletLifeTime = 1.0f;

    [Header("Invinsible After Damage")]
    public bool invinsibleAfterDamage = false;
    public float invinsibliltyTime = 2.0f;

    // Player Stats
    [Header("Energy Stats")]
    public float energy = 20;
    public float energyGainRate = 1;
    public float dashEnergy = 3;
    public float shootEnergy = 1;

    // Enemy Stats
    [Header("Enemy Damage")]
    public int touchDamage = 5;
    public int shootDamage = 10;
    public float shootTime = 3.0f;

    public EntityData() { }
    public EntityData(int entityId, EntitySO entitySO)
    {
        this.entityId = entityId;
        this.entityName = entitySO.entityName;
        this.teamId = entitySO.teamId;

        this.health = entitySO.health;
        this.speed = entitySO.speed;
        this.sprintSpeed = entitySO.sprintSpeed;

        this.jumpForce = entitySO.jumpForce;

        this.dashCooldown = entitySO.dashCooldown;
        this.dashSpeed = entitySO.dashSpeed;

        this.bulletDamage = entitySO.bulletDamage;
        this.bulletSpeed = entitySO.bulletSpeed;
        this.bulletLifeTime = entitySO.bulletLifeTime;

        this.invinsibleAfterDamage = entitySO.invinsibleAfterDamage;
        this.invinsibliltyTime = entitySO.invinsibliltyTime;

        // Player Stats
        this.energy = entitySO.energy;
        this.energyGainRate = entitySO.energyGainRate;
        this.dashEnergy = entitySO.dashEnergy;
        this.shootEnergy = entitySO.shootEnergy;

        // Enemy Stats
        this.touchDamage = entitySO.touchDamage;
        this.shootTime = entitySO.shootTime;
    }
}
