using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.0f, sensitivity = 5.0f, upDownRange = 6.0f, pickupRange = 5.0f;

    public float moveScale = 1.0f;
    public float rotationScale = 1.0f;
    public float gravityScale = 1.0f;

    private float forwardSpeed = 0.0f, sideSpeed = 0.0f, verticalRotation = 0.0f, verticalVelocity = 0.0f;

    private CharacterController characterController;

    private bool playerEnabled = true;

    //jacobs rotation code for controller
    private float rotY;
    private float rotX;

    private List<float> rotYlist;
    private List<float> rotXlist;

    public float max_x_turn = 15.0f;

    [SerializeField]
    int rotation_smooth = 5;

    // Use this for initialization
    void Start()
    {
        // Disable the cursor
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

        // Get Components
        characterController = GetComponent<CharacterController>();
        rotY = transform.rotation.eulerAngles.x;
        rotX = transform.rotation.eulerAngles.y;

        //RotatePlayer(new Vector3(0.0f, 180.0f, 0.0f));

        rotXlist = new List<float>();
        rotYlist = new List<float>();

    }

	void RotatePlayer(Vector3 angle)
	{
		Quaternion rot = Quaternion.Euler(angle.x, angle.y, angle.y);
		transform.Rotate (angle);//  new Vector3(angle.x, angle.y, angle.z);
	}

    // Update is called once per frame
    void Update()
    {
        // If the player is active
        if(playerEnabled)
        {

            // New rotational code to allow for delta time and controller input
            float mouseY = -Input.GetAxis("Mouse Y");
            float mouseX = Input.GetAxis("Mouse X");
            
            rotY += mouseY * sensitivity * Time.deltaTime * rotationScale;
            rotX = mouseX * sensitivity * Time.deltaTime * rotationScale; 

            //if(rotX > 0.0f)
            //{
            rotXlist.Add(rotX);
            //}
           

            transform.Rotate(new Vector3(0.0f, rotX, 0.0f));

            // Quaternion xRotation = Quaternion.Euler(0.0f, rotX, 0.0f);
            //transform.localRotation = xRotation;
            rotY = Mathf.Clamp(rotY, -upDownRange, upDownRange);


            Quaternion localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
            Camera.main.transform.localRotation = localRotation;
            // Move the camera
            // Movement
            forwardSpeed = Input.GetAxis("Vertical") * moveSpeed * moveScale;
            sideSpeed = Input.GetAxis("Horizontal") * moveSpeed * moveScale;

            // Stop the player falling if they're grounded
            verticalVelocity = characterController.isGrounded ? -10.0f * gravityScale : verticalVelocity + (Physics.gravity.y * gravityScale * Time.deltaTime);

            Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
            speed = transform.rotation * speed;
            characterController.Move(speed * Time.deltaTime);
        }
    }

	void DisablePlayer()
    {
        playerEnabled = false;
    }

    void EnablePlayer()
    {
        playerEnabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.0f, 2.0f, 1.0f));
    }

}