using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broogr : Enemy
{
    protected override void GetEnemyInput()
    {
        if (GameManager.instance.localPlayer.gravityBody.gravityAttractor != gravityBody?.gravityAttractor)
        {
            transform.LookAt(GameManager.instance.localPlayer.transform, GameManager.instance.localPlayer.transform.up);
        }
        base.GetEnemyInput();
    }

    protected override void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, shootOrigin.transform.position + (0.15f * shootOrigin.transform.forward), transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Initialize(this, entityData.bulletDamage, entityData.bulletSpeed, entityData.bulletLifeTime, gun.transform, true);
    }

    public override void DealDamage(int damage)
    {
        if (temporarilyInvinsible) return;
        base.DealDamage(damage);

        // After Damage, teleport Broogr to a new spawn point
        int largestPowerOfTwo = WaveSystem.FindLargestPowerOfTwoLessThanOrEqualToNumber(GameManager.instance.currentWave.waveNumber + 1);
        int moonCount = largestPowerOfTwo + 1;
        Vector3 spawnPoint = GameManager.instance.GetRandomSpawn(moonCount);
        transform.position = spawnPoint;
    }
}
