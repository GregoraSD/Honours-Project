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

                intInputs.Add(new Parameter<int>());
                intInputs.Add(new Parameter<int>());
                intOutputs.Add(new Parameter<int>());
			}
			
			// Define Invoke Method
			public override void Invoke(Sequencer invoker)
			{
                intOutputs[0].value = intInputs[0].value + intInputs[1].value;
            }

            public override void DrawGUI(Vector2 pan)
            {
                Rect box = new Rect(rect.position + pan, rect.size);
                GUI.Box(box, "");
                GUI.BeginGroup(new Rect(box));
                GUI.Label(new Rect(0.0f, 0.0f, box.width, box.height), id, UnityEditor.EditorStyles.centeredGreyMiniLabel);
                GUI.EndGroup();
            }
        }
	}
}
