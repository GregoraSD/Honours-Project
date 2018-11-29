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
            public Rect position = new Rect(0.0f, 0.0f, 150.0f, 50.0f);

            public List<Parameter<int>> intInputs = new List<Parameter<int>>();
            public List<Parameter<float>> floatInputs = new List<Parameter<float>>();
            public List<Parameter<string>> stringInputs = new List<Parameter<string>>();

            public List<Parameter<int>> intOutputs = new List<Parameter<int>>();
            public List<Parameter<float>> floatOutputs = new List<Parameter<float>>();
            public List<Parameter<string>> stringOutputs = new List<Parameter<string>>();

            public abstract void Invoke(Sequencer invoker);
            public abstract void DrawGUI(Vector2 pan);
        }
    }
}