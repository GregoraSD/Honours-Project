using UnityEngine;

namespace Sequencer
{
	namespace Node
	{
		public class Add : BaseNode
		{
            // Initialize Parameters
            public Add()
            {
                id = "Add";
                filter = "Math/";

                intInputs.Add(new Parameter<int>() { });
                intInputs.Add(new Parameter<int>() { });
                intOutputs.Add(new Parameter<int>());
			}
			
			// Define Invoke Method
			public override void Invoke(Sequencer invoker)
			{
                intOutputs[0].value = intInputs[0].value + intInputs[1].value;
                UnityEngine.Debug.Log(intInputs[0].value + " + " + intInputs[1].value + " = " + intOutputs[0].value);
            }

            public override void DrawGUI(Vector2 pan)
            {
                Rect box = new Rect(position.position + pan, position.size);
                GUI.Box(box, id);
                GUI.BeginGroup(new Rect(box));
                GUI.EndGroup();
            }
        }
	}
}
