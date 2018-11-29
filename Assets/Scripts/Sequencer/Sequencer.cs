using UnityEngine;
using System.Collections.Generic;

namespace Sequencer
{
    [System.Serializable]
    public class Sequencer : MonoBehaviour
    {
        public List<Node.BaseNode> nodes = new List<Node.BaseNode>();

        private void Start()
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Invoke(this);
            }
        }
    }
}