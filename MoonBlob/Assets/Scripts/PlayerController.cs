using System;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Transforms")]
    public Rigidbody playerRigidBody;
    public Transform playerCamera;

    [Header("Input")]
    private float x;
    private float y;
    private float mouseHorizontal;
    private float mouseVertical;
    private bool jumping;
    private bool sprinting;

    private bool dashHorizontal;

    [Header("Movement Variables")]
    private Vector3 velocity;
    public float speed;

    [Header("Rotation Variables")]
    private float xRotation;
    private float sensitivity = 200f;

    [Header("Jump Variables")]
    public float jumpForce = 500f;

    [Header("Grounded Variables")]
    private bool grounded = true;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Dash Variables")]
    private bool dashing = false;
    private bool readyToDash = true;
    public float dashCooldown = 1;
    public float dashSpeed = 5;

    [Header("Button Cooldowns")]
    public float buttonCooldown = 0.5f;
    private float buttonTapCooldown;
    private float buttonTapCount;
    private string tapButton;

    //-----------------------------------------------------------------------------------//
    //PlayerController Initialization and Update
    //-----------------------------------------------------------------------------------//
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetPlayerInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetPlayerInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");
        jumping = Input.GetButton("Jump");

        // Double Tap Dash Logic
        string buttonDown = "-";
        bool horizontal = Input.GetButtonDown("Horizontal");
        bool vertical = Input.GetButtonDown("Vertical");

        if (horizontal && x > 0)
        {
            buttonDown = "Dash Right";
        }
        else if (horizontal && x < 0)
        {
            buttonDown = "Dash Left";
        }
        else if (vertical && y > 0)
        {
            buttonDown = "Dash Forward";
        }
        else if (vertical && y < 0)
        {
            buttonDown = "Dash Backward";
        }

        if (buttonDown == tapButton)
        {
            if (buttonTapCooldown > 0 && buttonTapCount == 1)
            {
                dashing = true;
                buttonTapCooldown = 0.5f;
                buttonTapCount = 0;
            }
            else
            {
                buttonTapCooldown = 0.5f;
                buttonTapCount += 1;
            }

            if (buttonTapCooldown > 0)
            {
                buttonTapCooldown -= 1 * Time.deltaTime;
            }
            else
            {
                buttonTapCount = 0;
                tapButton = String.Empty;
            }
        }
        else if (buttonDown != "-")
        {
            tapButton = buttonDown;
        }
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Movement Functions
    //-----------------------------------------------------------------------------------//
    private void Move()
    {
        if (jumping)
        {
            Jump();
        }

        CalculateVelocity();

        // Applies velocity movement while also handling collisions
        playerRigidBody.MovePosition(transform.position + velocity);
    }

    private void Jump()
    {
        if (!grounded) return;
        
        grounded = false;

        //Apply jump forces
        playerRigidBody.AddForce(Vector3.up * jumpForce * 1.5f);
        jumping = false;   
    }

    private Vector3 Dash()
    {
        if (!dashing || !readyToDash) return Vector3.zero;
        readyToDash = false;
        dashing = false;

        Vector3 dashVector = Vector3.zero;
        if (tapButton == "Dash Right")
        {
            dashVector = transform.right * dashSpeed;
        }
        else if (tapButton == "Dash Left")
        {
            dashVector = -transform.right * dashSpeed;
        }
        else if (tapButton == "Dash Forward")
        {
            dashVector = transform.forward * dashSpeed;
        }
        else if (tapButton == "Dash Backward")
        {
            dashVector = -transform.forward * dashSpeed;
        }

        Invoke(nameof(ResetDash), dashCooldown);
        return dashVector;
    }

    private void CalculateVelocity()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 dash = Vector3.zero;
        if (readyToDash && dashing)
        {
            dash = Dash();
        }

        // Apply Velocity to the rigid body while also handling collisions
        Vector3 direction = (transform.forward * y) + (transform.right * x);
        velocity = (direction * speed * Time.fixedDeltaTime) + dash;
    }

    private void ResetDash()
    {
        readyToDash = true;
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Rotation Functions
    //-----------------------------------------------------------------------------------//
    private void Look()
    {
        float horizontalLook = mouseHorizontal * sensitivity * Time.deltaTime;
        float verticalLook = mouseVertical * sensitivity * Time.deltaTime;

        // Calculate Rotations
        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;
        float yRotation = rotation.y + horizontalLook;

        xRotation -= verticalLook;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * yRotation);
    }
    //-----------------------------------------------------------------------------------//

}
