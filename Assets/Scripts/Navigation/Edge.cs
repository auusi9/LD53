using UnityEngine;

namespace Navigation
{
    public class EdgePosition
    {
        public GraphNode Node1;
        public GraphNode Node2;
        public float Percentage;
        public Vector3 Position;

        public EdgePosition(GraphNode node1, GraphNode node2, float percentage, Vector3 position)
        {
            Node1 = node1;
            Node2 = node2;
            Percentage = percentage;
            Position = position;
        }

        public override bool Equals(object obj)
        {
            if(obj is EdgePosition other)
            {
                if (other.Node1 == this.Node1 && other.Node2 == this.Node2)
                    return true;
                if(other.Node1 == this.Node2 && other.Node2 == this.Node1)
                    return true;
            }
            return false;
        }

        public bool Contains(GraphNode graphNode)
        {
            return Node1 == graphNode || Node2 == graphNode;
        }
    }
}