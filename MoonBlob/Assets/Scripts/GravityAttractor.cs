using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public int moonId;
    public float gravitationalConstant = -5.0f;  //-9.81f;
    public Collider trigger;

    public Vector3 gravityDirection;

    //-----------------------------------------------------------------------------------//
    //Attraction Logic
    //-----------------------------------------------------------------------------------//
    public void Attract(Rigidbody attractedBody)
    {
        //Vector3 pullVector = FindSurface(attractedBody);
        //OrientBody(attractedBody, pullVector);

        gravityDirection = (attractedBody.transform.position - transform.position).normalized;
        OrientBodyOppositeGravity(attractedBody, gravityDirection);
        attractedBody.AddForce(gravitationalConstant * gravityDirection);
    }

    private void OrientBodyOppositeGravity(Rigidbody attractedBody, Vector3 gravityDirection)
    {
        Quaternion rotation = Quaternion.FromToRotation(attractedBody.transform.up, gravityDirection) * attractedBody.rotation;
        attractedBody.transform.rotation = Quaternion.Lerp(attractedBody.transform.rotation, rotation, 0.1f);
    }

    // Currently using gravity direction to orienty body rather than surface normal
    private Vector3 FindSurface(Rigidbody attracedBody)
    {
        float distance = Vector3.Distance(this.transform.position, attracedBody.transform.position);

        Vector3 surfaceNormal = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(attracedBody.transform.position, -attracedBody.transform.up, out hit, distance))
        {
            surfaceNormal = hit.normal;
        }
        return surfaceNormal;
    }

    private void OrientBody(Rigidbody attractedBody, Vector3 surfaceNormal)
    {
        attractedBody.transform.rotation = Quaternion.FromToRotation(attractedBody.transform.up, surfaceNormal) * attractedBody.rotation;
    }    
    //-----------------------------------------------------------------------------------//
}
