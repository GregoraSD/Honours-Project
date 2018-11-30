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

                intInputs.Add(new Parameter<int>());
                intOutputs.Add(new Parameter<int>());
                rect = new Rect(0.0f, 0.0f, rect.width / 2, rect.height / 2);
            }
			
			// Define Invoke Method
			public override void Invoke(Sequencer invoker)
			{
                intOutputs[0] = intInputs[0];
			}

            public override void DrawGUI(Vector2 pan)
            {
                Rect box = new Rect(rect.position + pan, rect.size);
                GUI.Box(box, "");
                GUI.BeginGroup(new Rect(box));
                GUI.Label(new Rect(0.0f, 0.0f, rect.width, rect.height), id, UnityEditor.EditorStyles.centeredGreyMiniLabel);
                GUI.EndGroup();
            }
        }
	}
}
