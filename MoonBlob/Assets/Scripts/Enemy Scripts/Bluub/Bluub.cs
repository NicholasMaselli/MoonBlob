using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Bluub : Enemy
{
    [Header("Bluub Shooting Variables")]
    public GameObject secondGun;
    public GameObject secondShootOrigin;

    public GameObject thirdGun;
    public GameObject thirdShootOrigin;

    public GameObject fourthGun;
    public GameObject fourthShootOrigin;

    private int hitsBeforeTeleport = 3;
    private int currentHits = 0;

    protected override void GetEnemyInput()
    {
        if (GameManager.instance.localPlayer.gravityBody.gravityAttractor != gravityBody?.gravityAttractor)
        {
            transform.LookAt(GameManager.instance.localPlayer.transform, GameManager.instance.localPlayer.transform.up);
        }
        base.GetEnemyInput();
    }

    protected override void RotateGun()
    {
        gun.transform.LookAt(GameManager.instance.localPlayer.transform);
        secondGun.transform.LookAt(GameManager.instance.localPlayer.transform);
        thirdGun.transform.LookAt(GameManager.instance.localPlayer.transform);
        fourthGun.transform.LookAt(GameManager.instance.localPlayer.transform);
    }

    protected override void Shoot()
    {
        GameObject bullet1GO = Instantiate(bulletPrefab, shootOrigin.transform.position, transform.rotation);
        Bullet bullet1 = bullet1GO.GetComponent<Bullet>();
        bullet1.Initialize(this, entityData.bulletDamage, entityData.bulletSpeed, entityData.bulletLifeTime, gun.transform, true);

        GameObject bullet2GO = Instantiate(bulletPrefab, secondShootOrigin.transform.position, transform.rotation);
        Bullet bullet2 = bullet2GO.GetComponent<Bullet>();
        bullet2.Initialize(this, entityData.bulletDamage, entityData.bulletSpeed, entityData.bulletLifeTime, secondGun.transform, true);

        GameObject bullet3GO = Instantiate(bulletPrefab, thirdShootOrigin.transform.position, transform.rotation);
        Bullet bullet3 = bullet3GO.GetComponent<Bullet>();
        bullet3.Initialize(this, entityData.bulletDamage, entityData.bulletSpeed, entityData.bulletLifeTime, thirdGun.transform, true);

        GameObject bullet4GO = Instantiate(bulletPrefab, fourthShootOrigin.transform.position, transform.rotation);
        Bullet bullet4 = bullet4GO.GetComponent<Bullet>();
        bullet4.Initialize(this, entityData.bulletDamage, entityData.bulletSpeed, entityData.bulletLifeTime, fourthGun.transform, true);
    }

    public override void DealDamage(int damage)
    {
        if (temporarilyInvinsible) return;

        base.DealDamage(damage);

        // After Damage, teleport Broogr to a new spawn point
        currentHits += 1;
        if (currentHits >= hitsBeforeTeleport)
        {
            int largestPowerOfTwo = WaveSystem.FindLargestPowerOfTwoLessThanOrEqualToNumber(GameManager.instance.currentWave.waveNumber + 1);
            int moonCount = largestPowerOfTwo + 1;
            Vector3 spawnPoint = GameManager.instance.GetRandomSpawn(moonCount);
            transform.position = spawnPoint;

            currentHits = 0;

            // Time for moon explosions!
            List<int> possibleMoons = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            int moonExplosionCount = 4;
            if (entityData.health <= 1000)
            {
                moonExplosionCount = 6;
            }
            else if (entityData.health <= 3000)
            {
                moonExplosionCount = 5;
            }            

            // Select moonExplosionCount random moons
            List<GravityAttractor> gravityAttractors = new List<GravityAttractor>();
            GameManager.instance.Shuffle(possibleMoons);
            for (int i = 0; i < moonExplosionCount; i++)
            {
                gravityAttractors.Add(GameManager.instance.moons[possibleMoons[i]]);
            }

            List<MeshRenderer> moonMeshRenders = new List<MeshRenderer>();
            List<Color> moonMaterialColor = new List<Color>();
            float timeStep = 0.5f;

            // Flicker Moons that are about to explode for 5 seconds!
            bool applyBaseColor = false;
            moonExplosionSequence = DOTween.Sequence();
            foreach (GravityAttractor gravityAttractor in gravityAttractors)
            {
                MeshRenderer gravityRenderer = gravityAttractor.gameObject.GetComponent<MeshRenderer>();

                moonMeshRenders.Add(gravityRenderer);
                moonMaterialColor.Add(gravityRenderer.material.color);

                Color baseColor = gravityRenderer.material.color;
                gravityRenderer.material = new Material(gravityRenderer.material);

                for (float i = 0.0f; i <= 5.0f; i += timeStep)
                {
                    if (!applyBaseColor)
                    {
                        moonExplosionSequence.Insert(i, gravityRenderer.material.DOColor(Color.white, timeStep)).SetEase(Ease.Linear);
                    }
                    else
                    {
                        moonExplosionSequence.Insert(i, gravityRenderer.material.DOColor(baseColor, timeStep)).SetEase(Ease.Linear);
                    }
                    applyBaseColor = !applyBaseColor;
                }
            }

            // Explode the moons and deal 30 damage to the player if they are being attracted by the moon
            moonExplosionSequence.Play().OnComplete(() =>
            {
                if (moonMeshRenders != null)
                {
                    int index = 0;
                    foreach (MeshRenderer meshRenderer in moonMeshRenders)
                    {
                        if (meshRenderer != null)
                        {
                            meshRenderer.material.color = moonMaterialColor[index];
                        }
                        index += 1;
                    }
                }

                foreach (GravityAttractor gravityAttractor in gravityAttractors)
                {
                    gravityAttractor.visualEffect.Play();
                    if (GameManager.instance.localPlayer.gravityBody.gravityAttractor == gravityAttractor)
                    {
                        GameManager.instance.localPlayer.ResetInvinsibility();
                        GameManager.instance.localPlayer.DealDamage(30);
                    }
                }
                GameManager.instance.sfxSource.PlayOneShot(StateManager.instance.dataDB.explosion, 1.0f);
            });
        }
    }
}
