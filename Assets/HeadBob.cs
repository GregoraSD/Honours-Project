using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob: MonoBehaviour 
{

	[SerializeField]
	private float bobSpeed = 0.18f, bobAmount = 0.2f, midpoint = 2.0f;

    [SerializeField]
    private bool ignoreLowerBob = false;

    [SerializeField]
    private Vector3 rotationStrength, rotationClamp, idleScale;

    [SerializeField]
    private float reverseRotationClamp;

    [SerializeField]
    private Vector2 randomScale = Vector2.one;

    private Vector3 rotationSum = Vector3.zero;
    private Vector3 bobRotationSum = Vector3.zero;
    private Vector3 offsets = Vector3.zero;
    private Vector3 rotationTimer = Vector3.zero;
	private float bobTimer = 0.0f;
    private float lastPoint = 0.0f;
    private float randomHeightScale = 1.0f;
    private float randomRotationScale = 1.0f;

    private void Start()
    {
        enabled = false;
        enabled = true;
    }

    void Update () 
	{
        float moveScale = FindObjectOfType<PlayerController>().moveScale;

        // Reset offset
        float heightOffset = 0.0f;
        float rotationOffset = 0.0f;

		// Fetch inputs
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		// Current position
		Vector3 camPosition = transform.localPosition;

        // If the player is not moving (i.e. no horizontal/vertical input)
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            bobTimer = 0.0f;

        // Otherwise
        else
        {
            // Calculate the height offset from a sin wave
            heightOffset = Mathf.Sin(bobTimer) * randomHeightScale * moveScale;
            rotationOffset = Mathf.Sin(bobTimer * 2.0f) * randomRotationScale;

            // Downward step
            if ((heightOffset) < 0.0f && (lastPoint > 0.0f))
            {
                randomHeightScale = Random.Range(randomScale.x, randomScale.y);
                randomRotationScale = Random.Range(randomScale.x, randomScale.y);
            }

            // Upward step
            else if ((heightOffset > 0.0f) && (lastPoint < 0.0f))
            {
                randomHeightScale = Random.Range(randomScale.x, randomScale.y);
                randomRotationScale = Random.Range(randomScale.x, randomScale.y);
            }

            // Increase the bob timer
            bobTimer += bobSpeed * Time.deltaTime;

            // Timer reset check
            if (bobTimer > Mathf.PI * 2)
                bobTimer -= Mathf.PI * 2;

            lastPoint = heightOffset;
        }
		
		// If their is a change in height
		if (heightOffset != 0) 
		{
            // Calculate total amount of height change (based on bob amount)
            float translateChange = heightOffset * bobAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

			// Limit translation change based on axis input
			totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;
            bobRotationSum.x = -rotationOffset * bobAmount * 5.0f * Mathf.Abs(vertical);
            bobRotationSum.z = rotationOffset * bobAmount * 3.0f * Mathf.Abs(horizontal);

            // Ignore lower head bob
            if(ignoreLowerBob)
                if ((midpoint + translateChange) < midpoint)
                    translateChange = -translateChange;

			// Set the new cam position
			camPosition.y = midpoint + translateChange;

            if(Mathf.Abs(horizontal) > 0.0f)
            {
                rotationTimer.z = 0.0f;

                // Left
                if (Input.GetKey(KeyCode.A) || Input.GetAxis("Vertical") != 0.0f)
                {
                    rotationSum.z += rotationStrength.z * Time.deltaTime * 0.1f * Input.GetAxis("Vertical");
                    if (rotationSum.z > rotationClamp.z)
                        rotationSum.z = rotationClamp.z;
                }

                // Right
                if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") != 0.0f)
                {
                    rotationSum.z -= rotationStrength.z * Time.deltaTime * 0.1f * Input.GetAxis("Horizontal");
                    if (rotationSum.z < -rotationClamp.z)
                        rotationSum.z = -rotationClamp.z;
                }
            }

            if(Mathf.Abs(vertical) > 0.0f)
            {
                rotationTimer.x = 0.0f;

                // Forward
                if (Input.GetKey(KeyCode.W))
                {
                    rotationSum.x += rotationStrength.x * Time.deltaTime * 0.1f;
                    if (rotationSum.x > rotationClamp.x)
                        rotationSum.x = rotationClamp.x;
                }

                // Backward
                if (Input.GetKey(KeyCode.S))
                {
                    rotationSum.x -= rotationStrength.x * Time.deltaTime * 0.1f;
                    if (rotationSum.x < -reverseRotationClamp)
                        rotationSum.x = -reverseRotationClamp;
                }
            }
		}

		// Otherwise
		else
        {
            camPosition.y = midpoint;
        }

        // Idling
        float noise = Mathf.PerlinNoise(Time.time / 5.0f, 0.0f) - 0.5f;
        offsets.x = noise * idleScale.x;
        offsets.y = noise * idleScale.y;
        offsets.z = noise * idleScale.z;

        // Set final camera position
        transform.localPosition = camPosition;
        transform.Rotate(rotationSum * moveScale, Space.Self);
        transform.Rotate(bobRotationSum * moveScale, Space.Self);
        transform.Rotate(offsets, Space.Self);

        rotationSum.x = Mathf.Lerp(rotationSum.x, 0.0f, rotationTimer.x * 0.5f);
        rotationSum.z = Mathf.Lerp(rotationSum.z, 0.0f, rotationTimer.z * 0.5f);

        rotationTimer.x += Time.deltaTime * 0.1f;
        rotationTimer.y += Time.deltaTime * 0.1f;
        rotationTimer.z += Time.deltaTime * 0.1f;
    }

    public Vector3 GetIdleScale()
    {
        return idleScale;
    }
}