using System;
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

        private static bool previousCompileState;
        private static MonoScript file;
        private static string path;
        private Node.BaseNode targetNode;

        private enum HoverType { None, Background, Function, Parameter };
        private HoverType currentHover;

        [MenuItem("Assets/Create/Sequencer Node")]
        public static void CreateNodeTemplate()
        {
            UnityEngine.Object obj = Selection.activeObject;
            path = obj == null ? "Assets" : AssetDatabase.GetAssetPath(obj.GetInstanceID());

            if(!Directory.Exists(path))
            {
                for(int i = path.Length - 1; i >= 0; i--)
                {
                    if(path[i] == '/')
                    {
                        path = path.Remove(i);
                        break;
                    }
                }
            }

            path += "/";
            file = new MonoScript();
            ProjectWindowUtil.CreateAsset(file, path + "New Node.cs");
            EditorApplication.update += WaitForAssetRefresh;
        }

        private static void WaitForAssetRefresh()
        {
            // Wait for a compiler state change
            if(EditorApplication.isCompiling != previousCompileState)
            {
                // Not Compiling -> Compiling
                if(EditorApplication.isCompiling)
                {
                    Debug.Log(path + file.name + ".cs");
                    File.WriteAllText(path + file.name + ".cs", String.Empty);

                    using (StreamWriter writer = new StreamWriter(path + file.name + ".cs"))
                    {
                        writer.WriteLine("namespace Sequencer");
                        writer.WriteLine("{");
                        writer.WriteLine("\tnamespace Node");
                        writer.WriteLine("\t{");
                        writer.WriteLine("\t\tpublic class " + file.name + " : BaseNode");
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\t// Initialize Parameters");
                        writer.WriteLine("\t\t\tpublic " + file.name + "()");
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\tid = " + '"' + file.name + '"' + ";");
                        writer.WriteLine("\t\t\t}");
                        writer.WriteLine("\t\t\t");
                        writer.WriteLine("\t\t\t// Define Invoke Method");
                        writer.WriteLine("\t\t\tpublic override void Invoke(Sequencer invoker)");
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\t");
                        writer.WriteLine("\t\t\t}");
                        writer.WriteLine("\t\t}");
                        writer.WriteLine("\t}");
                        writer.WriteLine("}");
                    }

                    AssetDatabase.Refresh();
                    EditorApplication.update -= WaitForAssetRefresh;
                }
            }

            // Set previous compile state
            previousCompileState = EditorApplication.isCompiling;
        }

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