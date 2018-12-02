using System.Collections;
using System.Collections.Generic;
namespace Sequencer
{
    namespace Node
    {
        public class Add : BaseNode
        {
            public override void Init()
            {
                id = "Add";
                filter = "Math/";
            }

            public override void Invoke(Sequencer invoker)
            {
            }
        }
    }
}