using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class Trigger : MonoBehaviour
{
    [Header("Parameters")]
    public Vector3 size = Vector3.one;

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
        collider.center = new Vector3(0.0f, size.y / 2, 0.0f);
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
        // Adjust pos so that pivot is at the bottom
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + (size.y / 2), transform.position.z);

        Quaternion rot = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        Vector3[] points = new Vector3[8];
        points[0] = new Vector3(size.x / 2, 0.0f, size.z / 2);
        points[1] = new Vector3(size.x / 2, 0.0f, -size.z / 2);

        // Draw a wire cube at the location of the trigger
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, rot * size);
        
        for(int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawWireSphere((rot * points[i]) + transform.position, 0.1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(triggerEnterResponse != null)
        {
            for(int i = 0; i < triggerEnterResponse.GetPersistentEventCount(); i++)
            {
                if(triggerEnterResponse.GetPersistentTarget(i) != null)
                {
                    // Get component and object reference to determine final target transform
                    Component component = triggerEnterResponse.GetPersistentTarget(i) as Component;
                    GameObject obj = triggerEnterResponse.GetPersistentTarget(i) as GameObject;
                    Transform target = component == null ? obj.transform : component.gameObject.transform;

                    // Determine component name & method name of this event
                    string componentName = component == null ? "Game Object" : component.GetType().Name;
                    string methodName = triggerEnterResponse.GetPersistentMethodName(i).Length > 0 ? triggerEnterResponse.GetPersistentMethodName(i) : "No Function";

                    Vector3 triggerCenter = transform.position + new Vector3(0.0f, size.y / 2, 0.0f);
                    Vector3 toTargetCenter = triggerCenter + (target.position - triggerCenter) / 2;
                    Vector3 toTargetPerp = Vector3.Cross(target.position - triggerCenter, Vector3.up).normalized;

                    // Draw a line from the trigger to the event object
                    Handles.DrawDottedLine(triggerCenter, target.position, 2.0f);
                    Gizmos.DrawWireSphere(triggerCenter, 0.03f);
                    Gizmos.DrawWireSphere(target.position, 0.03f);

                    // Draw event labels
                    Handles.Label(toTargetCenter, "On Trigger Enter", EditorStyles.centeredGreyMiniLabel);
                    Handles.Label(target.position, target.name + " (" + componentName + "):", EditorStyles.whiteBoldLabel);
                    Handles.Label(target.position, "\n   -" + methodName, EditorStyles.whiteLabel);
                }
            }
        }
    }
}