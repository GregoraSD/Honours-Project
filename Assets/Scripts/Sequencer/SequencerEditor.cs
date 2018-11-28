using UnityEngine;
using UnityEditor;

namespace Sequencer
{
    [CustomEditor(typeof(Sequencer))]
    public class SequencerEditor : Editor
    {
        public GUISkin skin;
        private Vector2 pan = Vector2.zero;
        private Vector2 dragStart = Vector2.zero;
        private Rect sequencerArea;
        private Event currentEvent;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            GUI.skin = skin;
            sequencerArea = GUILayoutUtility.GetRect(0.0f, 300.0f, GUILayout.ExpandWidth(true));
            GUI.Box(sequencerArea, "");

            GUI.BeginGroup(new Rect(sequencerArea.x + 1, sequencerArea.y + 1, sequencerArea.width - 2, sequencerArea.height - 2));

            GUI.DrawTextureWithTexCoords(new Rect(-10000.0f + pan.x * 0.5f, -10000.0f + pan.y * 0.5f, sequencerArea.width - 2 + 20000.0f, sequencerArea.height - 2 + 20000.0f), skin.customStyles[0].normal.background, new Rect(0.0f, 0.0f, (sequencerArea.width + 20000.0f) / skin.customStyles[0].normal.background.width, (sequencerArea.height + 20000.0f) / skin.customStyles[0].normal.background.height));

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
                        if (currentEvent.button == 2)
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

                    case EventType.ContextClick:
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("New Node"), false, CreateNewNode);
                        menu.ShowAsContext();
                        break;
                }
            }
        }

        private void CreateNewNode()
        {
            Debug.Log("Hey!");
            Repaint();
        }
    }
}