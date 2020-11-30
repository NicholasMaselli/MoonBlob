using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public Rigidbody gravityRigidBody;
    [HideInInspector] public GravityAttractor gravityAttractor;

    private void Start()
    {
        gravityAttractor = GameManager.instance.moons[0];
    }

    private void FixedUpdate()
    {
        if (gravityAttractor != null && gravityRigidBody != null)
        {
            gravityAttractor.Attract(gravityRigidBody);
        }
    }

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
            }
        }
    }
    //-----------------------------------------------------------------------------------//
}
