using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooboo : Enemy
{
    protected override void GetEnemyInput()
    {
        base.GetEnemyInput();
        if (GameManager.instance.localPlayer.gravityBody.gravityAttractor != gravityBody?.gravityAttractor)
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }
    }
}
