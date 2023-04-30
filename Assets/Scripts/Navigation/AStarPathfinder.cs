using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
public class AStarPathfinder
{
    public static List<Vector3> FindPath(Vector3 start, Vector3 goal, List<GraphNode> graphNodes)
    {
        GraphNode startNode = FindNearestNode(start, graphNodes);
        GraphNode goalNode = FindNearestNode(goal, graphNodes);

        List<GraphNode> openList = new List<GraphNode>();
        List<GraphNode> closedList = new List<GraphNode>();
        Dictionary<GraphNode, GraphNode> cameFrom = new Dictionary<GraphNode, GraphNode>();
        Dictionary<GraphNode, float> gScore = new Dictionary<GraphNode, float>();
        Dictionary<GraphNode, float> fScore = new Dictionary<GraphNode, float>();

        foreach (GraphNode node in graphNodes)
        {
            gScore[node] = Mathf.Infinity;
            fScore[node] = Mathf.Infinity;
        }

        gScore[startNode] = 0f;
        fScore[startNode] = HeuristicCostEstimate(startNode, goalNode);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            GraphNode current = GetLowestFScoreNode(openList, fScore);

            if (current == goalNode)
            {
                return ReconstructPath(cameFrom, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (GraphNode neighbor in current.SiblingNodes)
            {
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + Vector2.Distance(current.transform.position, neighbor.transform.position);

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goalNode);
            }
        }

        return null;
    }

    private static GraphNode FindNearestNode(Vector2 position, List<GraphNode> graphNodes)
    {
        GraphNode nearestNode = null;
        float minDistance = Mathf.Infinity;

        foreach (GraphNode node in graphNodes)
        {
            float distance = Vector2.Distance(position, node.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    private static float HeuristicCostEstimate(GraphNode a, GraphNode b)
    {
        return Vector2.Distance(a.transform.position, b.transform.position);
    }

    private static GraphNode GetLowestFScoreNode(List<GraphNode> openList, Dictionary<GraphNode, float> fScore)
    {
        GraphNode lowestNode = openList[0];
        float lowestFScore = fScore[lowestNode];

        for (int i = 1; i < openList.Count; i++)
        {
            GraphNode node = openList[i];
            float fScoreNode = fScore[node];

            if (fScoreNode < lowestFScore)
            {
                lowestNode = node;
                lowestFScore = fScoreNode;
            }
        }

        return lowestNode;
    }
        
    public static List<Vector3> ReconstructPath(Dictionary<GraphNode, GraphNode> cameFrom, GraphNode current)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(current.transform.position);
    
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current.transform.position);
        }
    
        path.Reverse();
        return path;
    }

    }
}