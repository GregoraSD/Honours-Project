using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Trigger))]
public class TriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get reference to trigger object and draw it's default GUI
        Trigger trigger = (Trigger)target;
        base.OnInspectorGUI();

        // Helper functions header
        EditorGUILayout.LabelField("Helper Functions", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        // Floor Button (Move the trigger to the ground)
        if(GUILayout.Button("Floor"))
        {
            // Store hit information
            RaycastHit hit;

            // Raycast downwards from the object position
            if (Physics.Raycast(trigger.transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                // Set position to collision point
                trigger.transform.position = hit.point + new Vector3(0.0f, 0.002f, 0.0f);
            }
        }

        // Reset Button (Reset the size back to 1, 1, 1)
        if(GUILayout.Button("Reset"))
        {
            trigger.size = Vector3.one;
            trigger.transform.localScale = Vector3.one;
            trigger.transform.rotation = Quaternion.identity;
            SceneView.RepaintAll();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Rotate -90°"))
        {
            trigger.transform.Rotate(Vector3.up, -90.0f);
        }

        if(GUILayout.Button("Rotate 90°"))
        {
            trigger.transform.Rotate(Vector3.up, 90.0f);
        }

        GUILayout.EndHorizontal();
    }
}