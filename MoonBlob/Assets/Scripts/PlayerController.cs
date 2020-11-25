using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Inputs")]
    private float horizontal;
    private float vertical;

    [Header("Physics Variables")]
    public float walkSpeed = 6.0f;
    private Vector3 velocity;

    private void Update()
    {
        GetPlayerInputs();
    }

    private void FixedUpdate()
    {
        CalculateVelocity();
        transform.Translate(velocity, Space.World);
    }

    private void GetPlayerInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void CalculateVelocity()
    {
        velocity = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.fixedDeltaTime * walkSpeed;
    }
}
