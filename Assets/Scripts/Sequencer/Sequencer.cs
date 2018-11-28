using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Sequencer
{
    [System.Serializable]
    public class Sequencer : MonoBehaviour
    {
        [SerializeField]
        public List<Node> nodes = new List<Node>();

        private void Start()
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                Debug.Log(nodes[i].function);
                Debug.Log(nodes[i].input.ints.Length);
                Debug.Log(nodes[i].output.ints.Length);

                //nodes[i].function.Invoke(nodes[i].input, nodes[i].output);
            }
        }
    }
}