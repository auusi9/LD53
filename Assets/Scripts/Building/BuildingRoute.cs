using System.Collections.Generic;
using InputHandling;
using Navigation;
using UnityEngine;

namespace Building
{
    public class BuildingRoute : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private Graph _graph;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private InputHandler _inputHandler;

        private bool _navigationCompleted = false;
        private List<GraphNode> _graphPath = new List<GraphNode>();
        private EdgePosition _edgePosition;
        private GraphNode _lastNode;
        private bool _selected = false;

        public void SelectPath()
        {
            _selected = true;
            _inputHandler.EditingRoute();
        }

        public void ResetPath()
        {
            _navigationCompleted = false;
            _graphPath.Clear();
        }

        public void DeselectPath()
        {
            _selected = false;
            if (!_navigationCompleted)
            {
                _graphPath.Clear();
                _lineRenderer.positionCount = 0;
            }
            _inputHandler.FinishedEditingRoute();
        }

        private void Update()
        {
            if (_selected)
            {
                if (Input.GetMouseButton(0) && !_navigationCompleted)
                {
                    if (_graphPath.Count == 0)
                    {
                        _graphPath.Add(_building.Node);
                        _lastNode = _building.Node;
                    }
                    
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = transform.position.z;

                    EdgePosition edgePosition = _graph.GetClosestPointOnEdges(mousePosition);
                    if(edgePosition != null)
                        _edgePosition = edgePosition;

                    GraphNode graphNode = _graph.GetNearestNode(mousePosition);

                    if (graphNode == _building.Node && _graphPath.Count > 1 && _lastNode.SiblingNodes.Contains(_building.Node))
                        _navigationCompleted = true;
                    
                    if(graphNode == null)
                        return;

                    if (_lastNode != graphNode)
                    {
                        if (_edgePosition.Contains(graphNode))
                        {
                            if (_graphPath.Contains(graphNode))
                            {
                                int i = _graphPath.Count - 1;
                                while (_graphPath.Count > 0 && _graphPath[i] != graphNode)
                                {
                                    _graphPath.RemoveAt(i);
                                    i--;
                                }
                                _lastNode = graphNode;
                            }
                            else if (_lastNode.SiblingNodes.Contains(graphNode))
                            {
                                _graphPath.Add(graphNode);
                                _lastNode = graphNode;
                            }
                        }
                    }
                }
                UpdateLineRenderer();
            }
        }

        private void UpdateLineRenderer()
        {
            if(_graphPath.Count == 0)
                return;
            
            Vector3[] array = new Vector3[_graphPath.Count + 1];
            for (var i = 0; i < _graphPath.Count; i++)
            {
                var node = _graphPath[i];
                array[i] = node.transform.position;
            }

            array[^1] = _edgePosition.Position;

            _lineRenderer.positionCount = array.Length;
            _lineRenderer.SetPositions(array);
        }

        public void SetBuilding(Building building)
        {
            _building = building;
        }
    }
}