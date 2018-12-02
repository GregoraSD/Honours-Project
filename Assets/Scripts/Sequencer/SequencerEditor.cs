﻿using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

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

        private Node.BaseNode targetNode;
        private enum HoverType { None, Background, Function, Parameter };
        private HoverType currentHover;

        public override void OnInspectorGUI()
        {
            sequencer = (Sequencer)target;
            EditorGUILayout.Space();
            GUI.skin = Resources.Load("Sequencer") as GUISkin;
            sequencerArea = GUILayoutUtility.GetRect(0.0f, 300.0f, GUILayout.ExpandWidth(true));
            GUI.Box(sequencerArea, "");
            GUI.BeginGroup(new Rect(sequencerArea.x + 1, sequencerArea.y + 1, sequencerArea.width - 2, sequencerArea.height - 2));
            GUI.DrawTextureWithTexCoords(new Rect(-10000.0f + pan.x, -10000.0f + pan.y, sequencerArea.width - 2 + 20000.0f, sequencerArea.height - 2 + 20000.0f), GUI.skin.customStyles[0].normal.background, new Rect(0.0f, 0.0f, (sequencerArea.width + 20000.0f) / GUI.skin.customStyles[0].normal.background.width, (sequencerArea.height + 20000.0f) / GUI.skin.customStyles[0].normal.background.height));

            for (int i = 0; i < sequencer.nodes.Count; i++)
            {
                sequencer.nodes[i].DrawGUI(pan);
            }

            GUI.EndGroup();
            EditorGUILayout.Space();

            currentEvent = Event.current;
            currentHover = GetCurrentHoverType();

            switch(currentHover)
            {
                case HoverType.Background:

                    if(currentEvent.type == EventType.MouseDown)
                    {
                        dragStart = currentEvent.mousePosition;
                    }

                    else if(currentEvent.type == EventType.MouseDrag)
                    {
                        if (currentEvent.button == 2)
                        {
                            pan += (currentEvent.mousePosition - dragStart);
                            dragStart = currentEvent.mousePosition;
                            currentEvent.Use();
                        }

                        if (targetNode != null && currentEvent.button == 0)
                        {
                            targetNode.rect.position += (currentEvent.mousePosition - dragStart);
                            dragStart = currentEvent.mousePosition;
                            currentEvent.Use();
                        }
                    }

                    else if (currentEvent.type == EventType.ContextClick)
                    {
                        Type[] nodeTypes = Assembly.GetAssembly(typeof(Node.BaseNode)).GetTypes().Where(t => t.IsSubclassOf(typeof(Node.BaseNode))).ToArray();
                        GenericMenu menu = new GenericMenu();

                        for (int i = 0; i < nodeTypes.Length; i++)
                        {
                            Node.BaseNode node = (Node.BaseNode)CreateInstance(nodeTypes[i]);
                            node.rect.position = new Vector2(-sequencerArea.x + currentEvent.mousePosition.x - pan.x, -sequencerArea.y + currentEvent.mousePosition.y - pan.y);
                            menu.AddItem(new GUIContent(node.filter + node.id), false, CreateNode, node);
                        }

                        menu.AddItem(new GUIContent("Clear"), false, sequencer.nodes.Clear);

                        menu.ShowAsContext();
                        currentEvent.Use();
                        EditorUtility.SetDirty(sequencer);
                    }

                    else if (currentEvent.type == EventType.MouseUp)
                    {
                        targetNode = null;
                    }

                    break;

                case HoverType.Function:

                    if(currentEvent.type == EventType.MouseDown)
                    {
                        if(targetNode == null && currentEvent.button == 0)
                        {
                            targetNode = NodeThatContainsMouse();
                        }

                        dragStart = currentEvent.mousePosition;
                    }

                    else if(currentEvent.type == EventType.MouseDrag)
                    {
                        if(targetNode != null && currentEvent.button == 0)
                        {
                            targetNode.rect.position += (currentEvent.mousePosition - dragStart);
                            dragStart = currentEvent.mousePosition;
                            currentEvent.Use();
                        }

                        if(currentEvent.button == 2)
                        {
                            pan += (currentEvent.mousePosition - dragStart);
                            dragStart = currentEvent.mousePosition;
                            currentEvent.Use();
                        }
                    }

                    else if (currentEvent.type == EventType.MouseUp)
                    {
                        targetNode = null;
                    }

                    break;
            }

            Repaint();
        }

        private void HandleInput()
        {
            if (sequencerArea.Contains(currentEvent.mousePosition))
            {
                switch (currentEvent.type)
                {
                    case EventType.MouseDown:
                        if(targetNode == null && currentEvent.button == 0)
                        {
                            targetNode = NodeThatContainsMouse();
                        }

                        dragStart = currentEvent.mousePosition;
                        break;

                    case EventType.MouseDrag:
                        if(targetNode != null && currentEvent.button == 0)
                        {
                            targetNode.rect.position += (currentEvent.mousePosition - dragStart);
                            dragStart = currentEvent.mousePosition;
                            currentEvent.Use();
                        }   

                        if (currentEvent.button == 2)
                        {
                            pan += (currentEvent.mousePosition - dragStart);
                            dragStart = currentEvent.mousePosition;
                            currentEvent.Use();
                        }
                        break;

                    case EventType.ContextClick:

                        // Get list of node types
                        Type[] nodeTypes = Assembly.GetAssembly(typeof(Node.BaseNode)).GetTypes().Where(t => t.IsSubclassOf(typeof(Node.BaseNode))).ToArray();
                        GenericMenu menu = new GenericMenu();

                        // Create node menu items
                        for (int i = 0; i < nodeTypes.Length; i++)
                        {
                            Node.BaseNode node = (Node.BaseNode)CreateInstance(nodeTypes[i]);
                            node.rect.position = new Vector2(-sequencerArea.x + currentEvent.mousePosition.x - pan.x, -sequencerArea.y + currentEvent.mousePosition.y - pan.y);
                            menu.AddItem(new GUIContent(node.filter + node.id), false, CreateNode, node);
                        }

                        menu.AddItem(new GUIContent("Clear"), false, sequencer.nodes.Clear);

                        menu.ShowAsContext();
                        currentEvent.Use();
                        EditorUtility.SetDirty(sequencer);
                        break;

                    case EventType.MouseUp:
                        targetNode = null;
                        break;
                }
            }
        }

        private void CreateNode(System.Object node)
        {
            sequencer.nodes.Add((Node.BaseNode)node);
        }

        private void RemoveNode(System.Object node)
        {
            sequencer.nodes.Remove((Node.BaseNode)node);
        }

        private Node.BaseNode NodeThatContainsMouse()
        {
            for(int i = sequencer.nodes.Count - 1; i >= 0; i--)
            {
                if(sequencer.nodes[i].rect.Contains(-sequencerArea.position + currentEvent.mousePosition - pan))
                {
                    return sequencer.nodes[i];
                }
            }

            return null;
        }

        private HoverType GetCurrentHoverType()
        {
            if(sequencerArea.Contains(currentEvent.mousePosition))
            {
                for (int i = sequencer.nodes.Count - 1; i >= 0; i--)
                {
                    if (sequencer.nodes[i].rect.Contains(-sequencerArea.position + currentEvent.mousePosition - pan))
                    {
                        return HoverType.Function;
                    }
                }

                return HoverType.Background;
            }

            return HoverType.None;
        }

    }//B00l43nV01DWUZH3R3
}