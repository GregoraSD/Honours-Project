using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [Header("Parameters")]
    public Vector3 size = Vector3.one;
    public Color color = Color.green;

    [Header("Event Responses")]
    public UnityEvent triggerEnterResponse;
    public UnityEvent triggerExitResponse;
    public UnityEvent triggerStayResponse;

    private void Start()
    {
        // Create the in-game collider and set its parameters
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = size;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger Entered
        triggerEnterResponse.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        // Trigger Exited
        triggerExitResponse.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        // Trigger Stayed
        triggerStayResponse.Invoke();
    }

    private void OnDrawGizmos()
    {
        // Draw a wire cube at the location of the trigger
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, size);
    }
}