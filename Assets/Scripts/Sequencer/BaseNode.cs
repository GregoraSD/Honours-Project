using UnityEngine;
using System.Collections.Generic;

namespace Sequencer
{
    namespace Node
    {
        public abstract class BaseNode : ScriptableObject
        {
            public string id;
            public string filter;
            public Rect rect = new Rect(0.0f, 0.0f, 150.0f, 50.0f);

            public List<Parameter<int>> intInputs = new List<Parameter<int>>();
            public List<Parameter<float>> floatInputs = new List<Parameter<float>>();
            public List<Parameter<string>> stringInputs = new List<Parameter<string>>();

            public List<Parameter<int>> intOutputs = new List<Parameter<int>>();
            public List<Parameter<float>> floatOutputs = new List<Parameter<float>>();
            public List<Parameter<string>> stringOutputs = new List<Parameter<string>>();

            public BaseNode()
            {
                Init();
            }

            public virtual void Init()
            {
                id = "New Node";
                filter = "Undefined/";
            }

            public abstract void Invoke(Sequencer invoker);

            public virtual void DrawGUI(Vector2 pan)
            {
                Rect box = new Rect(rect.position + pan, rect.size);
                GUI.Box(box, "");

                GUI.BeginGroup(box);
                GUI.Label(new Rect(0.0f, 0.0f, box.width, box.height), id, UnityEditor.EditorStyles.centeredGreyMiniLabel);
                GUI.EndGroup();
            }
        }
    }
}