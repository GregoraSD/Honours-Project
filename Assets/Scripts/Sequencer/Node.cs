using UnityEngine;

namespace Sequencer
{
    [System.Serializable]
    public class Node : MonoBehaviour
    {
        public string name = "";
        public string method = "";
        public Input input = new Input();
        public Output output = new Output();

        public delegate void Function(Input i, Output o);
        public Function function;
    }
}