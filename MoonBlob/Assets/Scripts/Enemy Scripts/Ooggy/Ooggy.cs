﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ooggy : Enemy
{
    [Header("Ooggy Shooting Variables")]
    public GameObject secondGun;
    public GameObject secondShootOrigin;

    protected override void GetEnemyInput()
    {
        transform.LookAt(GameManager.instance.localPlayer.transform, GameManager.instance.localPlayer.transform.up);
        if (GameManager.instance.localPlayer.gravityBody.gravityAttractor == gravityBody?.gravityAttractor)
        {
            y = 1;
        }
    }

    protected override void RotateGun()
    {
        gun.transform.LookAt(GameManager.instance.localPlayer.transform);
        secondGun.transform.LookAt(GameManager.instance.localPlayer.transform);
    }

    protected override void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, shootOrigin.transform.position + (0.15f * shootOrigin.transform.forward), transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Initialize(this, entityData.bulletDamage, entityData.bulletSpeed, entityData.bulletLifeTime, gun.transform);

        GameObject secondBulletGO = Instantiate(bulletPrefab, secondShootOrigin.transform.position + (0.15f * secondShootOrigin.transform.forward), transform.rotation);
        Bullet secondBullet = secondBulletGO.GetComponent<Bullet>();
        secondBullet.Initialize(this, entityData.bulletDamage, entityData.bulletSpeed, entityData.bulletLifeTime, secondGun.transform);
    }

    protected override void Die()
    {
        // Spawn 2 Globs on death
        GameManager.instance.dataDB.enemyPrefabs.TryGetValue("Glob", out GameObject globPrefrab);
        if (globPrefrab != null)
        {
            int globCount = 2;
            for (int i = 0; i < globCount; i++)
            {
                Instantiate(globPrefrab, transform.position, transform.rotation);
            }
        }
        Destroy(this.gameObject);
    }
}
