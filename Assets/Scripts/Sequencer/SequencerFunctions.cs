using UnityEngine;

namespace Sequencer
{
    [System.Serializable]
    public static class SequencerFunctions
    {
        public static void AddNode(Sequencer sequencer)
        {
            Node node = new Node();
            node.input.ints = new int[2];
            node.output.ints = new int[1];
            node.name = "Add";
            sequencer.nodes.Add(node);
        }

        public static void ConstantNode(Sequencer sequencer)
        {
            Node node = new Node();
            node.input.floats = new float[1];
            node.output.floats = new float[1];
            node.name = "Constant";
            sequencer.nodes.Add(node);
        }

        public static void ConstantFunction(Input i, Output o)
        {
            o.floats[0] = i.floats[0];
        }

        public static void AddFunction(Input i, Output o)
        {
            Debug.Log(i.ints[0] + " + " + i.ints[1] + " = " + o.ints[0]);
            o.ints[0] = i.ints[0] + i.ints[1];
        }

        public static void MultiplyNode(Sequencer sequencer)
        {
            Node node = new Node();
            node.input.ints = new int[2];
            node.output.ints = new int[1];
            node.name = "Multiply";
            sequencer.nodes.Add(node);
        }

        public static void MultiplyFunction(Input i, Output o)
        {
            o.ints[0] = i.ints[0] * i.ints[1];
        }
    }
}