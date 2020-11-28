using UnityEngine;

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
    private bool dashLeft;
    private bool dashRight;
    private bool shoot;

    [Header("Movement Variables")]
    public float speed;

    [Header("Rotation Variables")]
    private float xRotation;
    private float sensitivity = 200f;

    [Header("Jump Variables")]
    public float jumpForce = 500f;

    [Header("Grounded Variables")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool grounded = true;

    [Header("Dash Variables")]
    public float dashCooldown = 1;
    public float dashSpeed = 5;
    private bool readyToDash = true;

    [Header("Shooting Variables")]
    public GameObject bulletPrefab;

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
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);
        GetPlayerInput();
        Look();

        // Jumping and dashing in Update because they are single physics actions
        if (jumping)
        {
            Jump();
        }
        Dash();

        if (shoot)
        {
            Shoot();
        }
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
        jumping = Input.GetButtonDown("Jump");
        dashLeft = Input.GetButtonDown("Dash Left");
        dashRight = Input.GetButtonDown("Dash Right");

        shoot = Input.GetMouseButtonDown(0);
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Movement Functions
    //-----------------------------------------------------------------------------------//
    private void Move()
    {
        PlayerMovement();
    }
    private void PlayerMovement()
    {
        Vector3 direction = (transform.forward * y) + (transform.right * x).normalized;
        Vector3 velocity = direction * speed * Time.fixedDeltaTime;
        playerRigidBody.MovePosition(transform.position + velocity);
    }

    private void Jump()
    {
        if (!grounded) return;        
        grounded = false;

        // Apply jump forces
        playerRigidBody.AddForce(transform.up * jumpForce * 1.5f);
        jumping = false;   
    }

    private void Dash()
    {
        if (!readyToDash || (!dashLeft && !dashRight)) return;
        readyToDash = false;

        Vector3 dashVector = Vector3.zero;
        if (dashLeft)
        {
            dashVector = -transform.right * dashSpeed;
        }
        else if (dashRight)
        {
            dashVector = transform.right * dashSpeed;
        }

        Invoke(nameof(ResetDash), dashCooldown);
        playerRigidBody.MovePosition(transform.position + dashVector);
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

    //-----------------------------------------------------------------------------------//
    //Shooting Functions
    //-----------------------------------------------------------------------------------//
    public void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position + (0.25f * transform.forward) + (0.15f * transform.up), transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.bulletRigidBody.velocity = transform.forward * bullet.speed;
        bullet.playerController = this;
    }
    //-----------------------------------------------------------------------------------//

}
