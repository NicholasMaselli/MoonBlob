using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVideoRotation : MonoBehaviour
{
    public Transform pivot;

    public float rotationspeed = 80.0f;
    public float radiusSpeed = 0.5f;
    private float radius;

    public void Start()
    {
        Vector3 cameraPivotVector = transform.position - pivot.position;
        radius = cameraPivotVector.magnitude;        
    }

    public void Update()
    {
        transform.LookAt(pivot, transform.up);
        transform.RotateAround(pivot.position, transform.up, rotationspeed * Time.deltaTime);

        var desiredPosition = (transform.position - pivot.position).normalized * radius + pivot.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }
}
