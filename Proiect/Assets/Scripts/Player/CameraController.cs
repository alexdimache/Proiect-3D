using UnityEngine;

public class CameraController : MonoBehaviour
{
    //mouse sensitivity
	private float mouseSensitivity=2.0f;
    //reference for the player object (body)
    private Rigidbody playerReference;
    
    //used to prevent the camera from flipping over
    private float clampValueX = 0;

    void Start()
    {
        //reference of the parent object
        playerReference = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update ()
    {
        RotateCamera();
	}

    private void RotateCamera()
    {
        //getting the raw axis from input
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        

        Vector3 cameraRotation = transform.rotation.eulerAngles;
        Vector3 bodyRotation = playerReference.rotation.eulerAngles;

        cameraRotation.x -= mouseY * mouseSensitivity;
        clampValueX -= mouseY * mouseSensitivity;
        bodyRotation.y += mouseX * mouseSensitivity;
        cameraRotation.z = 0;

        //prevent it from flipping when looking straight down
        if (clampValueX > 90)
        {
            clampValueX = 90;
            cameraRotation.x = 90;
        }else
            //prevent it from flipping when looking straight up
            if (clampValueX < -90)
            {
                clampValueX = -90;
                cameraRotation.x = 270;
            }
        //applying the rotation for the camera on the X axis 
        //and for the body as well, on the Y axis
        transform.rotation = Quaternion.Euler(cameraRotation);
        playerReference.MoveRotation(Quaternion.Euler(bodyRotation));
    }

    //used to change the mouse sensitivity
    public void ChangeSensitivity(float givenSensitivity)
    {
        mouseSensitivity = givenSensitivity;
    }
}
