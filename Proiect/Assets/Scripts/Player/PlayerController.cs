using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //the player's body itself
    private Rigidbody playerBody;
    //the camera object
    private GameObject playerCamera;

    //variable to check whether the player is on the ground or not
    private bool isGrounded = false;

    private float gravity = 20.0f;
    private float jumpForce = 7.5f;
    private bool canJump = true;
    private float timeSinceLastJump = -1;

    private float runSpeed = 10.0f;
    private float walkSpeed = 5.0f;
    private float currentSpeed;

	// Use this for initialization
	void Start ()
    {
        playerBody = GetComponent<Rigidbody>();
        playerCamera = transform.Find("PlayerCamera").gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (isGrounded)
            GroundMovement();
        else
            AirMovement();
	}

    //called when the player touches a collider
    private void OnCollisionEnter(Collision collision)
    {
        while (collision.gameObject.tag == "Ground" && isGrounded == false)
        {
            isGrounded = true;
            Debug.Log("GROUNDED: " + isGrounded.ToString());
        }

        canJump = true;
    }

    //every tile is a apart of the ground
    private void OnCollisionStay(Collision collision)
    {
        if (!isGrounded)
            isGrounded = true;
    }

    //called when the player exits the collision
    private void OnCollisionExit(Collision collision)
    {
        while (collision.gameObject.tag == "Ground" && isGrounded == true)
        {
            isGrounded = false;
            Debug.Log("GROUNDED: " + isGrounded.ToString());
        }
    }

    private void GroundMovement()
    {
        if (Input.GetButton("Jump"))
        {
            Jump();
            return;
        }

        if (Input.GetButton("Walk"))
            currentSpeed = walkSpeed;
        else
            currentSpeed = runSpeed;

        float strafeMovement = Input.GetAxis("Horizontal") * currentSpeed;
        float forwardMovement = Input.GetAxis("Vertical") * currentSpeed;

        Vector3 movementForce = new Vector3(strafeMovement, 0, forwardMovement);
        movementForce = transform.TransformDirection(movementForce);
        movementForce -= playerBody.velocity;

        playerBody.AddForce(movementForce, ForceMode.VelocityChange);

    }

    private void Jump()
    {
        //the player cannot jump more than once every 1.5 seconds
        if (timeSinceLastJump != -1 && Time.time - timeSinceLastJump < 1f)
            return;

        //the player can only jump once in the air
        if (!isGrounded)
        {
            canJump = false;
            jumpForce = gravity + 5.0f;
        }
        else
        jumpForce = 7.5f; //going back to the normal jump force


        Debug.Log("JUMP " + (Time.time - timeSinceLastJump));
        timeSinceLastJump = Time.time;
        playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
    }

    private void AirMovement()
    {
        if (Input.GetButton("Jump") && canJump)
            Jump();

        //applying my own gravity
        Vector3 fallingForce = Vector3.down * gravity;
        playerBody.AddForce(fallingForce, ForceMode.Acceleration);

        float strafeMovement = Input.GetAxis("Horizontal");
        playerBody.AddForce(strafeMovement * Vector3.right, ForceMode.Force);
        
    }
}
