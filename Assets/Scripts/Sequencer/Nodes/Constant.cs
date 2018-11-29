using UnityEngine;

namespace Sequencer
{
	namespace Node
	{
		public class Constant : BaseNode
		{
			// Initialize Parameters
			public Constant()
			{
				id = "Constant";
                filter = "Value/";

                intInputs.Add(new Parameter<int>() { });
                intOutputs.Add(new Parameter<int>());
            }
			
			// Define Invoke Method
			public override void Invoke(Sequencer invoker)
			{
                intOutputs[0] = intInputs[0];
                UnityEngine.Debug.Log(intOutputs[0].value + " = " + intInputs[0].value);
			}

            public override void DrawGUI(Vector2 pan)
            {
                Rect box = new Rect(position.position + pan, position.size / 2);
                GUI.Box(box, id);
                GUI.BeginGroup(new Rect(box));
                GUI.EndGroup();
            }
        }
	}
}
