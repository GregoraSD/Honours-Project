using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [Header("Parameters")]
    public Vector3 size = Vector3.one;

    [Header("Event Responses")]
    public UnityEvent triggerEnterResponse;
    public UnityEvent triggerExitResponse;
    public UnityEvent triggerStayResponse;

    public Color triggerColor = Color.green;

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Get current rotation from the object
        Quaternion rot = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        // Define Cube Points
        Vector3[] points = new Vector3[8];
        points[0] = new Vector3(size.x / 2, 0.0f, size.z / 2);
        points[1] = new Vector3(size.x / 2, 0.0f, -size.z / 2);
        points[2] = new Vector3(-size.x / 2, 0.0f, size.z / 2);
        points[3] = new Vector3(-size.x / 2, 0.0f, -size.z / 2);
        points[4] = new Vector3(size.x / 2, size.y, size.z / 2);
        points[5] = new Vector3(size.x / 2, size.y, -size.z / 2);
        points[6] = new Vector3(-size.x / 2, size.y, size.z / 2);
        points[7] = new Vector3(-size.x / 2, size.y, -size.z / 2);

        // Scale & Rotate
        for(int i = 0; i < points.Length; i++)
        {
            points[i].Scale(transform.localScale);
            points[i] = rot * points[i];
        }

        // Set Color
        Gizmos.color = triggerColor;

        // Draw lines in cube shape
        Gizmos.DrawLine(transform.position + points[0], transform.position + points[1]);
        Gizmos.DrawLine(transform.position + points[0], transform.position + points[2]);
        Gizmos.DrawLine(transform.position + points[1], transform.position + points[3]);
        Gizmos.DrawLine(transform.position + points[2], transform.position + points[3]);
        Gizmos.DrawLine(transform.position + points[4], transform.position + points[5]);
        Gizmos.DrawLine(transform.position + points[4], transform.position + points[6]);
        Gizmos.DrawLine(transform.position + points[5], transform.position + points[7]);
        Gizmos.DrawLine(transform.position + points[6], transform.position + points[7]);
        Gizmos.DrawLine(transform.position + points[0], transform.position + points[4]);
        Gizmos.DrawLine(transform.position + points[1], transform.position + points[5]);
        Gizmos.DrawLine(transform.position + points[2], transform.position + points[6]);
        Gizmos.DrawLine(transform.position + points[3], transform.position + points[7]);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw all event debug information
        DrawEventGizmos(triggerEnterResponse, "On Trigger Enter");
        DrawEventGizmos(triggerExitResponse, "On Trigger Exit");
        DrawEventGizmos(triggerStayResponse, "On Trigger Stay");
    }

    private void DrawEventGizmos(UnityEvent e, string typeLabel)
    {
        if (e != null)
        {
            for (int i = 0; i < e.GetPersistentEventCount(); i++)
            {
                if (e.GetPersistentTarget(i) != null)
                {
                    // Get component and object reference to determine final target transform
                    Component component = e.GetPersistentTarget(i) as Component;
                    GameObject obj = e.GetPersistentTarget(i) as GameObject;
                    Transform target = component == null ? obj.transform : component.gameObject.transform;

                    // Determine component name & method name of this event
                    string componentName = component == null ? "Game Object" : component.GetType().Name;
                    string methodName = e.GetPersistentMethodName(i).Length > 0 ? e.GetPersistentMethodName(i) : "No Function";

                    Quaternion currentRot = transform.rotation;
                    Vector3 currentPos = transform.position;
                    transform.position += new Vector3(0.0f, size.y / 2, 0.0f);
                    transform.LookAt(target);
                    Vector3 perpendicularUp = transform.up;
                    transform.rotation = currentRot;
                    transform.position = currentPos;

                    // Object vectors
                    Vector3 triggerCenter = transform.position + transform.up * (size.y / 2);
                    Vector3 lineCenter = triggerCenter + (target.position - triggerCenter) / 2;
                    Vector3 linePerpendicular = Vector3.Cross(target.position - triggerCenter, perpendicularUp).normalized * 0.5f;

                    // Draw a line from the trigger to the event object
                    Gizmos.DrawLine(triggerCenter, target.position);
                    Gizmos.DrawWireSphere(triggerCenter, 0.03f);
                    Gizmos.DrawWireSphere(target.position, 0.03f);

                    // Draw the mesh of the target (if it has one)
                    Mesh targetMesh = target.gameObject.GetComponent<MeshFilter>() == null ? null : target.gameObject.GetComponent<MeshFilter>().sharedMesh;
                    //Gizmos.DrawWireMesh(targetMesh, target.position, target.rotation, target.localScale);

                    int repeat = 5;
                    float distance = Vector3.Distance(triggerCenter, target.position) + 2 * repeat;

                    // Draw Arrows
                    for(int d = 1; d < Mathf.Round(distance / repeat); d++)
                    {
                        Vector3 point = triggerCenter + (target.position - triggerCenter) * (d / Mathf.Round(distance / repeat));
                        Gizmos.DrawLine(point, point + Quaternion.Euler(perpendicularUp * -45.0f) * linePerpendicular);
                        Gizmos.DrawLine(point, point - Quaternion.Euler(perpendicularUp * 45.0f) * linePerpendicular);
                    }

                    // Draw event labels
                    UnityEditor.Handles.Label(lineCenter, typeLabel, UnityEditor.EditorStyles.whiteMiniLabel);
                    UnityEditor.Handles.Label(target.position, target.name + " (" + componentName + "):", UnityEditor.EditorStyles.whiteBoldLabel);
                    UnityEditor.Handles.Label(target.position, "\n   -" + methodName, UnityEditor.EditorStyles.whiteLabel);
                }
            }
        }
    }
#endif
}