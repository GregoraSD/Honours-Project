using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sequencer))]
public class SequencerEditor : Editor
{
    public GUISkin skin;
    private Vector2 pan = Vector2.zero;
    private Vector2 dragStart = Vector2.zero;
    private Rect sequencerArea;
    private Event currentEvent;
    private Sequencer sequencer;

    public override void OnInspectorGUI()
    {
        sequencer = (Sequencer)target;
        EditorGUILayout.Space();
        GUI.skin = skin;
        sequencerArea = GUILayoutUtility.GetRect(0.0f, 200.0f, GUILayout.ExpandWidth(true));
        GUI.Box(sequencerArea, "");

        GUI.BeginGroup(new Rect(sequencerArea.x + 1, sequencerArea.y + 1, sequencerArea.width - 2, sequencerArea.height - 2));
        GUI.Box(new Rect(Vector2.zero + pan, new Vector2(100.0f, 50.0f)), "NODE");
        GUI.EndGroup();
        EditorGUILayout.Space();

        currentEvent = Event.current;
        HandleInput();
    }

    private void HandleInput()
    {
        if (sequencerArea.Contains(currentEvent.mousePosition))
        {
            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    Debug.Log("Here");
                    dragStart = currentEvent.mousePosition;
                    break;

                case EventType.MouseDrag:
                    if (currentEvent.button == 2)
                    {
                        pan += (currentEvent.mousePosition - dragStart);
                        dragStart = currentEvent.mousePosition;
                        currentEvent.Use();
                    }

                    break;
            }
        }
    }
}