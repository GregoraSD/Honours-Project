using UnityEngine;

namespace Sequencer
{
    [System.Serializable]
    public class Node
    {
        public string name = "";
        public Input input = new Input();
        public Output output = new Output();
    }
}