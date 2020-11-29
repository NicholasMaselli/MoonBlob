using UnityEngine;

public class Glob : Enemy 
{
    protected override void Update()
    {
        shootElapsedTime += Time.deltaTime;
        if (shootElapsedTime > entityData.shootTime)
        {
            Shoot();
            shootElapsedTime = 0.0f;
        }
        base.Update();
    }
}