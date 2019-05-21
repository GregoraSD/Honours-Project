using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.0f, sensitivity = 5.0f, upDownRange = 6.0f;

    [SerializeField]
    private float controllerSensitivity = 5.0f;

    public float moveScale = 1.0f;
    public float rotationScale = 1.0f;
    public float gravityScale = 1.0f;

    private float forwardSpeed = 0.0f, sideSpeed = 0.0f, verticalVelocity = 0.0f;

    private CharacterController characterController;

    private bool playerEnabled = true;

    //jacobs rotation code for controller
    private float rotY;
    private float rotX;

    private List<float> rotXlist;

    public float max_x_turn = 15.0f;

    public StepCycleCollection allStepCycles;
    public StepCycleGroup activeStepCycle;

    [SerializeField]
    private AudioSource stepAudioSource;

    [SerializeField]
    private float stepCooldown = 0.5f;

    private float stepTimer = 0.0f;

    [SerializeField]
    private PauseMenu pauseMenu;

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

        rotXlist = new List<float>();
    }

	void RotatePlayer(Vector3 angle)
	{
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
            float controllerX = Input.GetAxis("HorizontalTurn");
            float controllerY = Input.GetAxis("VerticalTurn");

            rotY += mouseY * sensitivity * Time.deltaTime * rotationScale + (controllerY * controllerSensitivity * Time.deltaTime);
            rotX = mouseX * sensitivity * Time.deltaTime * rotationScale + (controllerX * controllerSensitivity * Time.deltaTime); 

            rotXlist.Add(rotX);

            transform.Rotate(new Vector3(0.0f, rotX, 0.0f));

            rotY = Mathf.Clamp(rotY, -upDownRange, upDownRange);


            Quaternion localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
            Camera.main.transform.localRotation = localRotation;
            // Move the camera
            // Movement
            forwardSpeed = Input.GetAxis("Vertical") * moveSpeed * moveScale;
            sideSpeed = Input.GetAxis("Horizontal") * moveSpeed * moveScale;

            // If Moving
            if(forwardSpeed > 0 || forwardSpeed < 0 || sideSpeed > 0 || sideSpeed < 0)
            {
                stepTimer += Time.deltaTime;
                if(stepTimer > stepCooldown)
                {
                    RaycastHit hitInfo;
                    if(Physics.Raycast(transform.position, Vector3.down, out hitInfo, 2.0f))
                    {
                        StepCycleGroup hitGroup = allStepCycles.FindGroupWithTag(hitInfo.transform.gameObject.tag);
                        activeStepCycle = hitGroup == null ? activeStepCycle : hitGroup;
                    }

                    stepAudioSource.clip = activeStepCycle.GetRandomClip();
                    stepAudioSource.pitch = Random.Range(0.8f, 1.2f);
                    stepAudioSource.volume = activeStepCycle.volume;
                    stepAudioSource.Play();
                    stepTimer -= stepCooldown;
                }
            }
            else
            {
                stepTimer = 0.0f;
            }

            // Stop the player falling if they're grounded
            verticalVelocity = characterController.isGrounded ? -10.0f * gravityScale : verticalVelocity + (Physics.gravity.y * gravityScale * Time.deltaTime);

            Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
            speed = transform.rotation * speed;
            characterController.Move(speed * Time.deltaTime);

            if(Input.GetButtonDown("Pause"))
            {
                pauseMenu.Toggle();
            }
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

    public void SetActiveStepGroup(StepCycleGroup newGroup)
    {
        activeStepCycle = newGroup;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.0f, 2.0f, 1.0f));
    }
}