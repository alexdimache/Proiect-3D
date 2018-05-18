using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //the player's body itself
    private CharacterController playerBody;
    //the player's camera
    private Camera playerCamera;
    //the UI object
    public GameObject playerUIprefab;
    //the UI instance
    private GameObject playerUIInstance;

    // the gravity
    private const float gravity = 25.0f;
    // the jump force
    private const float jumpForce = 10.0f;
    // the running speed
    private const float runSpeed = 7.5f;
    // the walking speed
    private const float walkSpeed = 5.0f;
    // maximum multiplier for the speed while in air 
    private const float maxMultiplier = 2;
    // the player needs to jump 3 times consecutively to reach the maximum air speed
    private const float multiplierIncrement = 2.0f / 3;
    
    // flag to see if there's a jump queued
    private bool jumpQueued;
    // multiplier used to reach the maximum air speed
    private float airSpeedMultiplier;
    // value used to multiply the speed when moving diagonally
    private float speedMultiplier;
    // the current speed
    private float currentSpeed;
    

    //mouse sensitivity
    private float mouseSensitivity = 1.0f;
    //used to prevent the camera from flipping over
    private float clampValueX = 0;
    //using the mouse X and Y axis
    private float mouseX;
    private float mouseY;
    //using the input axis for vertical and horizontal movement
    private float strafeMovement;
    private float forwardMovement;
    //the movement direction and force
    private Vector3 movementForce = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        jumpQueued = false;

        playerBody = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        playerUIInstance = Instantiate(playerUIprefab);

        //using the pythagorean formula to get the diagonal movement proportion
        speedMultiplier = Mathf.Sqrt(runSpeed * runSpeed * 2) / runSpeed / 2;

        //TEMP VVV
        Cursor.lockState = CursorLockMode.Locked;
        //TEMP ^^^
    }
    // FixedUpdate is called 50 times per second (by default)
    void FixedUpdate()
    {
        RotateCamera();
        Movement();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Walk"))
            currentSpeed = walkSpeed;
        else
            currentSpeed = runSpeed;
    }

    //the camera function
    private void RotateCamera()
    {
        //getting the raw axis from input
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        //I'm getting the current rotation of the camera and the body
        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        Vector3 bodyRotation = playerBody.transform.rotation.eulerAngles;

        //the rotationon the X axis and Y axis
        cameraRotation.x -= mouseY * mouseSensitivity;
        clampValueX -= mouseY * mouseSensitivity;
        bodyRotation.y += mouseX * mouseSensitivity;
        cameraRotation.z = 0;

        //prevent it from flipping when looking straight down
        if (clampValueX > 90)
        {
            clampValueX = 90;
            cameraRotation.x = 90;
        }
        else
            //prevent it from flipping when looking straight up
            if (clampValueX < -90)
        {
            clampValueX = -90;
            cameraRotation.x = 270;
        }
        //applying the rotation for the camera on the X axis 
        //and for the body as well, on the Y axis
        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
        playerBody.transform.rotation = Quaternion.Euler(bodyRotation);
    }

    private void Movement()
    {
        //getting the axis for the movement
        forwardMovement = Input.GetAxis("Vertical");
        strafeMovement = Input.GetAxis("Horizontal");

        if (playerBody.isGrounded)
        {
            movementForce.x = strafeMovement * currentSpeed;
            movementForce.z = forwardMovement * currentSpeed;

            if (forwardMovement != 0 && strafeMovement != 0)
            {
                movementForce.x *= speedMultiplier;
                movementForce.z *= speedMultiplier;
            }

            if (Input.GetButtonDown("Jump") || jumpQueued)
            {
                Debug.Log("JUMP");

                if (airSpeedMultiplier < maxMultiplier)
                    airSpeedMultiplier += multiplierIncrement;

                if (jumpQueued)
                    jumpQueued = false;

                movementForce.y = jumpForce;
            }
            else
                airSpeedMultiplier = 1;
        }   
        else
        {
            if(forwardMovement == 0 && strafeMovement != 0)
            {
                movementForce.x = strafeMovement * runSpeed * airSpeedMultiplier;
                movementForce.z = 0;
            }
            else
            {
                movementForce.x = 0;
                movementForce.z = forwardMovement * runSpeed * airSpeedMultiplier;
            }

            if (Input.GetButtonDown("Jump"))
                jumpQueued = true;

            //if the player is not grounded I'm applying gravity
            movementForce.y -= gravity * Time.deltaTime;
        }

        //changing the direction of the movement to match the body's direction
        movementForce = transform.TransformDirection(movementForce);

        //moving the player
        playerBody.Move(movementForce * Time.deltaTime);
    }
}
