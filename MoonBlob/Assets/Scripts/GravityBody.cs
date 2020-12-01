using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public Rigidbody gravityRigidBody;
    [HideInInspector] public GravityAttractor gravityAttractor;

    //-----------------------------------------------------------------------------------//
    //Initialization and Update Functions
    //-----------------------------------------------------------------------------------//
    public void Initialize()
    {
        gravityAttractor = GameManager.instance.moons[0];
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameEnded) return;
        if (GameManager.instance.paused) return;

        if (gravityAttractor != null && gravityRigidBody != null)
        {
            gravityAttractor.Attract(gravityRigidBody);
        }
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Change Planet Functions
    //-----------------------------------------------------------------------------------//
    private void OnTriggerEnter(Collider collider)
    {
        if (collider?.transform != gravityAttractor?.transform)
        {
            GravityAttractor newGravityAttractor = collider.transform.gameObject.GetComponent<GravityAttractor>();
            if (newGravityAttractor != null)
            {
                gravityAttractor = newGravityAttractor;
                gravityAttractor.Attract(gravityRigidBody);

                Enemy enemy = gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    GameManager.instance.ChangeMoonWaveUI(enemy, gravityAttractor.moonId);
                }                
            }
        }
    }
    //-----------------------------------------------------------------------------------//
}
