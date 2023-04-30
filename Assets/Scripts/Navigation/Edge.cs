using System;
using UnityEngine;

namespace Navigation
{
    public class EdgePosition
    {
        public GraphNode Node1;
        public GraphNode Node2;
        public Vector3 Position;

        public EdgePosition(GraphNode node1, GraphNode node2, Vector3 position)
        {
            Node1 = node1;
            Node2 = node2;
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

        protected bool Equals(EdgePosition other)
        {
            if (other == null)
                return false;
            
            if (other.Node1 == this.Node1 && other.Node2 == this.Node2)
                return true;
            if(other.Node1 == this.Node2 && other.Node2 == this.Node1)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Node1, Node2);
        }

        public GraphNode GetClosestNode(Vector3 searchPosition)
        {
            float distanceNode1 = (searchPosition - Node1.transform.position).sqrMagnitude;
            float distanceNode2 = (searchPosition - Node2.transform.position).sqrMagnitude;
            
            return distanceNode1 >= distanceNode2 ? Node2 : Node1;
        }

        public bool Contains(GraphNode graphNode)
        {
            return Node1 == graphNode || Node2 == graphNode;
        }
    }
}