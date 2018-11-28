using UnityEngine;
using UnityEditor;

namespace Sequencer
{
    [CustomEditor(typeof(Sequencer))]
    public class SequencerEditor : Editor
    {
        private Vector2 pan = Vector2.zero;
        private Vector2 dragStart = Vector2.zero;
        private Rect sequencerArea;
        private Event currentEvent;
        private Sequencer sequencer;

        public override void OnInspectorGUI()
        {
            sequencer = (Sequencer)target;
            EditorGUILayout.Space();
            GUI.skin = Resources.Load("Sequencer") as GUISkin;
            sequencerArea = GUILayoutUtility.GetRect(0.0f, 300.0f, GUILayout.ExpandWidth(true));
            GUI.Box(sequencerArea, "");
            GUI.BeginGroup(new Rect(sequencerArea.x + 1, sequencerArea.y + 1, sequencerArea.width - 2, sequencerArea.height - 2));
            GUI.DrawTextureWithTexCoords(new Rect(-10000.0f + pan.x * 0.5f, -10000.0f + pan.y * 0.5f, sequencerArea.width - 2 + 20000.0f, sequencerArea.height - 2 + 20000.0f), GUI.skin.customStyles[0].normal.background, new Rect(0.0f, 0.0f, (sequencerArea.width + 20000.0f) / GUI.skin.customStyles[0].normal.background.width, (sequencerArea.height + 20000.0f) / GUI.skin.customStyles[0].normal.background.height));

            for (int i = 0; i < sequencer.nodes.Count; i++)
            {
                GUI.Box(new Rect(pan.x, pan.y, 100.0f, 30.0f), sequencer.nodes[i].name);
            }

            GUI.EndGroup();
            EditorGUILayout.Space();

            currentEvent = Event.current;
            HandleInput();
            Repaint();
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
                        menu.AddItem(new GUIContent("Add Node"), false, CreateAddNode);
                        menu.AddItem(new GUIContent("Multiply Node"), false, CreateMultiplyNode);
                        menu.AddItem(new GUIContent("Constant Node"), false, CreateConstantNode);
                        menu.AddItem(new GUIContent("Helper/Empty Sequencer"), false, EmptySequencer);
                        menu.ShowAsContext();
                        currentEvent.Use();
                        EditorUtility.SetDirty(sequencer);
                        break;
                }
            }
        }

        private void EmptySequencer()
        {
            sequencer.nodes.Clear();
        }

        private void CreateAddNode()
        {
            SequencerFunctions.AddNode(sequencer);
        }

        private void CreateMultiplyNode()
        {
            SequencerFunctions.MultiplyNode(sequencer);
        }

        private void CreateConstantNode()
        {
            SequencerFunctions.ConstantNode(sequencer);
        }

    }//B00l43nV01DWUZH3R3
}