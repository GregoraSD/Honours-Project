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
            }
        }
    }
}