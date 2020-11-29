﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntitySO", menuName = "EntitySO")]
public class EntitySO : ScriptableObject
{
    [Header("Identity")]
    public string entityName;

    [Header("Entity Stats")]
    public int health = 100;
    public float speed = 5;
    
    [Header("Jump Stats")]
    public float jumpForce = 200;

    [Header("Dash Stats")]
    public float dashCooldown = 1;
    public float dashSpeed = 3;

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
}
