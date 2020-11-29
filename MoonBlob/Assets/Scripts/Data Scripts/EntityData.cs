using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityData
{
    [Header("Entity Stats")]
    public int health = 100;
    public float speed = 5;

    [Header("Jump Stats")]
    public float jumpForce = 500;

    [Header("Dash Stats")]
    public float dashCooldown = 1;
    public float dashSpeed = 5;

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
    [Header("Touch Damage")]
    public int touchDamage = 5;

    public EntityData() { }
    public EntityData(EntitySO entitySO)
    {
        this.health = entitySO.health;
        this.speed = entitySO.speed;

        this.jumpForce = entitySO.jumpForce;

        this.dashCooldown = entitySO.dashCooldown;
        this.dashSpeed = entitySO.dashSpeed;

        this.invinsibleAfterDamage = entitySO.invinsibleAfterDamage;
        this.invinsibliltyTime = entitySO.invinsibliltyTime;

        // Player Stats
        this.energy = entitySO.energy;
        this.energyGainRate = entitySO.energyGainRate;
        this.dashEnergy = entitySO.dashEnergy;
        this.shootEnergy = entitySO.shootEnergy;

        // Enemy Stats
        this.touchDamage = entitySO.touchDamage;
    }
}
