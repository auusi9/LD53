using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Navigation;
using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "Graph", menuName = "Navigation/Graph")]
public class Graph : ScriptableObject
{
    [SerializeField] private BaseEvent _restartGame;

    private List<GraphNode> _graphNodes = new List<GraphNode>();
    public List<GraphNode> Nodes => _graphNodes;

    private void OnEnable()
    {
        _graphNodes.Clear();
        _restartGame.Register(RestartGame);
    }

    private void OnDisable()
    {
        _restartGame.UnRegister(RestartGame);
    }

    private void RestartGame()
    {
        _graphNodes.Clear();
    }

    public void AddNode(GraphNode graphNode)
    {
        if (!_graphNodes.Contains(graphNode))
        {
            _graphNodes.Add(graphNode);
        }

        for (var i = graphNode.SiblingNodes.Count - 1; i >= 0; i--)
        {
            var sibling = graphNode.SiblingNodes[i];

            if (sibling == null)
            {
                graphNode.SiblingNodes.RemoveAt(i);
            }
            else
            {
                if (!sibling.SiblingNodes.Contains(graphNode))
                {
                    sibling.SiblingNodes.Add(graphNode);
                }
            }
        }
    }
    
    public GraphNode GetNearestNode(Vector3 searchPosition)
    {
        GraphNode nearestNode = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GraphNode graphNode in _graphNodes)
        {
            float distance = (searchPosition - graphNode.transform.position).sqrMagnitude;

            if (distance < nearestDistance)
            {
                nearestNode = graphNode;
                nearestDistance = distance;
            }
        }

        return nearestNode;
    }

    public GraphNode GetGraphNodeInPosition(Vector3 searchPosition)
    {
        foreach (GraphNode graphNode in _graphNodes)
        {
            if (graphNode == null) continue;

            if (IsPositionInsideNode(searchPosition, graphNode))
            {
                return graphNode;
            }
        }

        return null;
    }
    
    public bool IsPositionInsideNode(Vector3 searchPosition, GraphNode graphNode)
    {
        float radius = graphNode.GetRadius();
        float distance = Vector3.Distance(searchPosition, graphNode.transform.position);

        if (distance <= radius)
        {
            return true;
        }

        return false;
    }
    
    public EdgePosition GetClosestPointOnEdges(Vector3 position)
    {
        float minDistance = Mathf.Infinity;
        EdgePosition closestPoint = null;

        List<int> checkedNodes = new List<int>();

        foreach (GraphNode node in _graphNodes)
        {
            if(checkedNodes.Contains(node.GetHashCode()))
                continue;
            
            foreach (GraphNode siblingNode in node.SiblingNodes)
            {
                if(checkedNodes.Contains(siblingNode.GetHashCode()))
                    continue;

                GraphNode nodeStart = node;
                GraphNode nodeEnd = siblingNode;
                
                Vector3 pointOnEdge = GetClosestPointOnEdge(position, nodeStart.transform.position, nodeEnd.transform.position, out float distanceInEdge);

                if (!IsPointBetweenPoints(nodeStart.transform.position, nodeEnd.transform.position, pointOnEdge))
                {
                    continue;
                }
                
                float distance = Vector3.Distance(position, pointOnEdge) ;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = new EdgePosition(nodeStart, nodeEnd, pointOnEdge);
                }
            }

            checkedNodes.Add(node.GetHashCode());
        }

        return closestPoint;
    }

    private Vector3 GetClosestPointOnEdge(Vector3 position, Vector3 start, Vector3 end, out float distance)
    {
        Vector3 direction = (end - start).normalized;
        distance = Vector3.Dot(position - start, direction);
        distance = Mathf.Clamp(distance, 0f, Vector3.Distance(start, end));
        return start + direction * distance;
    }
    
    public static bool IsPointBetweenPoints(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // Calculate the distance between p1 and p2
        float distance = Vector3.Distance(p1, p2);
    
        // Calculate the distance between p1 and p3
        float distance1 = Vector3.Distance(p1, p3);
    
        // Calculate the distance between p2 and p3
        float distance2 = Vector3.Distance(p2, p3);
    
        // Check if the sum of distance1 and distance2 is equal to distance
        return Mathf.Approximately(distance1 + distance2, distance);
    }

    public void RemoveNode(GraphNode graphNode)
    {
        for (var i = graphNode.SiblingNodes.Count - 1; i >= 0; i--)
        {
            var sibling = graphNode.SiblingNodes[i];

            if (sibling.SiblingNodes.Contains(graphNode))
            {
                sibling.SiblingNodes.Remove(graphNode);
            }
        }
        
        if(_graphNodes.Contains(graphNode))
            _graphNodes.Remove(graphNode);
    }
}
