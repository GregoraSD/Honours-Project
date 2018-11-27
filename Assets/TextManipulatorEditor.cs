using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextManipulator))]
public class TextManipulatorEditor : Editor
{
    public GUISkin skin;

    // Store inspection reference
    private TextManipulator textManipulator;
    private Event currentEvent;

    private Vector2 pan = Vector2.zero;
    private Vector2 dragStart = Vector2.zero;

    // Get inspection reference when enabled
    private void OnEnable() { textManipulator = (TextManipulator)target; pan = Vector2.zero; }
    private void OnDisable() { textManipulator = null; pan = Vector2.zero; }

    public override void OnInspectorGUI()
    {
        GUI.skin = skin;

        EditorGUILayout.Space();
        // Get current event and draw the manipulator area
        currentEvent = Event.current;
        Rect area = GUILayoutUtility.GetRect(0.0f, 250.0f, GUILayout.ExpandWidth(true));
        GUI.Box(area, "");
        EditorGUILayout.Space();

        GUI.BeginGroup(new Rect(pan, new Vector2(100000, 100000)));

        for (int i = 0; i < textManipulator.functions.Count; i++)
        {
            Rect functionArea = textManipulator.functions[i].area;

            if(currentEvent.type == EventType.Repaint)
            {
                Debug.Log("Background Area: " + area);

                if(functionArea.x + functionArea.width + pan.x > area.x + area.width)
                {
                    // Fix Right
                    functionArea.x = area.x + area.width - pan.x - functionArea.width;
                }

                if (area.x - pan.x > functionArea.x)
                {
                    // Fix Left
                    float dist = area.x - pan.x - functionArea.x;

                    functionArea.width -= dist;
                    functionArea.width = functionArea.width < 0.0f ? 0.0f : functionArea.width;
                    functionArea.x = area.x - pan.x;
                }

                if(functionArea.y + pan.y < area.y)
                {
                    // Fix top
                    functionArea.y = area.y - pan.y;
                }

                if(functionArea.y + functionArea.height + pan.y > area.y + area.height)
                {
                    // Fix bottom
                    functionArea.y = area.y + area.height - functionArea.height - pan.y;
                }
            }

            Debug.Log("Function Area: " + functionArea);
            GUI.Box(functionArea, "Test");
        }

        GUI.EndGroup();

        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                if (!area.Contains(currentEvent.mousePosition))
                    return;

                dragStart = currentEvent.mousePosition;
                break;

            case EventType.MouseDrag:
                if (!area.Contains(currentEvent.mousePosition))
                    return;

                pan += (currentEvent.mousePosition - dragStart);
                dragStart = currentEvent.mousePosition;
                Repaint();
                currentEvent.Use();
                break;

            case EventType.ContextClick:
                if (!area.Contains(currentEvent.mousePosition))
                    return;

                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("Create New Function"), false, CreateNewFunction);
                menu.ShowAsContext();

                currentEvent.Use();
                break;
        }
        Debug.Log("Pan: " + pan);
        Repaint();
    }

    void CreateNewFunction()
    {
        ManipulatorFunction newFunction = new ManipulatorFunction();
        newFunction.area = new Rect(currentEvent.mousePosition - pan, new Vector2(100.0f, 30.0f));
        textManipulator.functions.Add(newFunction);
    }
}