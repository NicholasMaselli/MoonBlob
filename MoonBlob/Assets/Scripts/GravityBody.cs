using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public Rigidbody gravityRigidBody;
    [HideInInspector] public GravityAttractor gravityAttractor;

    private void Start()
    {
        gravityAttractor = GameManager.instance.initialMoon;
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
    public void OnTriggerEnter(Collider collider)
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
