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
    private void OnEnable() { textManipulator = (TextManipulator)target; }
    private void OnDisable() { textManipulator = null; }

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
                #region Old Collision Code
                /*
                // RIGHT
                if(functionArea.x + functionArea.width + pan.x > area.x + area.width)
                {
                    float dist = functionArea.x + functionArea.width + pan.x - area.x - area.width;
                    functionArea.width -= dist;
                    functionArea.x += dist;
                    functionArea.width = functionArea.width < 0.0f ? 0.0f : functionArea.width;

                    functionArea.x = area.x + area.width - pan.x - functionArea.width;
                }

                // LEFT
                if (area.x - pan.x > functionArea.x)
                {
                    float dist = area.x - pan.x - functionArea.x;
                    functionArea.width -= dist;
                    functionArea.width = functionArea.width < 0.0f ? 0.0f : functionArea.width;
                    functionArea.x = area.x - pan.x;
                }

                // TOP
                if(functionArea.y + pan.y < area.y)
                {
                    float dist = area.y - functionArea.y - pan.y;
                    functionArea.height -= dist;
                    functionArea.height = functionArea.height < 0.0f ? 0.0f : functionArea.height;
                    functionArea.y = area.y - pan.y;
                }

                // BOTTOM
                if(functionArea.y + functionArea.height + pan.y > area.y + area.height)
                {
                    float dist = functionArea.y + functionArea.height + pan.y - area.y - area.height;
                    functionArea.height -= dist;
                    functionArea.y += dist;
                    functionArea.height = functionArea.height < 0.0f ? 0.0f : functionArea.height;

                    functionArea.y = area.y + area.height - functionArea.height - pan.y;
                }
                */
                #endregion
            }

            if(functionArea.width != 0.0f && functionArea.height != 0.0f)
            {
                GUI.Box(functionArea, "Test");
            }
        }

        GUI.EndGroup();

        // If mouse is inside of the background area
        if(area.Contains(currentEvent.mousePosition))
        {
            // Check if there is a function that contains the mouse
            ManipulatorFunction f = FunctionThatContainsMouse();

            // If function is null, then not hovering over a function
            if(f == null)
            {
                switch(currentEvent.type)
                {
                    case EventType.MouseDown:
                        dragStart = currentEvent.mousePosition;
                        break;

                    case EventType.MouseDrag:
                        pan += (currentEvent.mousePosition - dragStart);
                        dragStart = currentEvent.mousePosition;
                        Repaint();
                        currentEvent.Use();
                        break;

                    case EventType.ContextClick:
                        GenericMenu menu = new GenericMenu();

                        menu.AddItem(new GUIContent("Create New Function"), false, CreateNewFunction);
                        menu.AddItem(new GUIContent("Clear"), false, textManipulator.functions.Clear);
                        menu.ShowAsContext();

                        currentEvent.Use();
                        break;
                }
            }

            // Otherwise, hovering over a function
            else
            {
                switch (currentEvent.type)
                {
                    case EventType.MouseDown:
                        dragStart = currentEvent.mousePosition;
                        break;

                    case EventType.MouseDrag:
                        f.area.position += (currentEvent.mousePosition - dragStart);
                        dragStart = currentEvent.mousePosition;
                        Repaint();
                        currentEvent.Use();
                        break;
                }
            }
        }

        Repaint();

        /*
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
                menu.AddItem(new GUIContent("Clear"), false, textManipulator.functions.Clear);
                menu.ShowAsContext();

                currentEvent.Use();
                break;
        }
        */
    }

    private void CreateNewFunction()
    {
        ManipulatorFunction newFunction = new ManipulatorFunction();
        newFunction.area = new Rect(currentEvent.mousePosition - pan, new Vector2(100.0f, 30.0f));
        textManipulator.functions.Add(newFunction);
    }

    private void Clear()
    {
        textManipulator.functions.Clear();
    }

    private ManipulatorFunction FunctionThatContainsMouse()
    {
        for(int i = textManipulator.functions.Count - 1; i >= 0; i--)
        {
            if(textManipulator.functions[i].area.Contains(currentEvent.mousePosition - pan))
            {
                return textManipulator.functions[i];
            }
        }

        return null;
    }
}