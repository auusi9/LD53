using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class GraphNode : MonoBehaviour
{
    [SerializeField] private List<GraphSubNode> _subNodes;
    [SerializeField] private List<GraphNode> _siblingNodes;
    [SerializeField] private Graph _graph;

    public List<GraphNode> SiblingNodes => _siblingNodes;

    private void Start()
    {
        _graph.AddNode(this);
    }

    private void OnDestroy()
    {
        _graph.RemoveNode(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x / 2);

        foreach (var subNode in _subNodes)
        {
            Gizmos.DrawSphere(subNode.transform.position, subNode.transform.lossyScale.x);
        }
        
        if(_siblingNodes == null || _siblingNodes.Count == 0)
            return;
        
        for (var i = _siblingNodes.Count - 1; i >= 0; i--)
        {
            GraphNode sibling = _siblingNodes[i];
            if (sibling == null)
            {
                continue;
            }
            Gizmos.DrawLine(transform.position, sibling.transform.position);
        }
    }

    private void OnValidate()
    {
        for (var i = _siblingNodes.Count - 1; i >= 0; i--)
        {
            GraphNode sibling = _siblingNodes[i];
            if (sibling == null)
            {
                _siblingNodes.RemoveAt(i);
            }
        }
    }

    public float GetRadius()
    {
        return transform.lossyScale.x / 2;
    }

    public GraphSubNode GetPosition(int index)
    {
        if (_subNodes.Count > index)
        {
            return _subNodes[index];
        }
        
        GraphSubNode availableSubNode = _subNodes.FirstOrDefault(x => x.Available);

        if (availableSubNode == null)
        {
            return _subNodes[Random.Range(0, _subNodes.Count)];
        }

        return availableSubNode;
    }

    public int GetAvailableIndex()
    {
        GraphSubNode availableSubNode = _subNodes.FirstOrDefault(x => x.Available);

        if (availableSubNode == null)
        {
            return 0;
        }

        return _subNodes.IndexOf(availableSubNode);
    }
}
