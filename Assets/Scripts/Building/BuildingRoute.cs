﻿using System;
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
        private List<Route> _graphPath = new List<Route>();
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

            foreach (var path in _graphPath)
            {
                path.GraphSubNode.Empty();
            }
            
            _graphPath.Clear();
            _lineRenderer.positionCount = 0;
        }

        public void DeselectPath()
        {
            _selected = false;
            if (!_navigationCompleted)
            {
                ResetPath();
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
                        _graphPath.Add(new Route(_building.Node));
                        _lastNode = _building.Node;
                    }
                    
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = transform.position.z;

                    EdgePosition edgePosition = _graph.GetClosestPointOnEdges(mousePosition);
                    if(edgePosition != null)
                        _edgePosition = edgePosition;

                    //_edgePosition.Position = mousePosition;

                    GraphNode graphNode = _edgePosition.GetClosestNode(mousePosition);

                    if(graphNode == null)
                        return;

                    //if (_lastNode != graphNode)
                    {
                        if (_edgePosition.Contains(graphNode))
                        {
                            if (_graphPath.Count > 1 && _edgePosition.Contains(_graphPath[^2].GraphNode))
                            {
                                int i = _graphPath.Count - 1;
                                _graphPath[i].GraphSubNode.Empty();
                                _graphPath.RemoveAt(i);
                                _lastNode = _graphPath[^1].GraphNode;
                            }
                            else if (_lastNode.SiblingNodes.Contains(graphNode) && (_graph.IsPositionInsideNode(mousePosition, graphNode) || !_edgePosition.Contains(_lastNode)))
                            {
                                _graphPath.Add(new Route(graphNode));
                                _lastNode = graphNode;
                            }
                        }
                    }

                    if (graphNode == _building.Node && _graphPath.Count > 1 &&
                        _lastNode.SiblingNodes.Contains(_building.Node))
                    {
                        _navigationCompleted = true;
                        _graphPath.Add(new Route(_building.Node));
                    }
                }
                UpdateLineRenderer();
            }
        }

        private void UpdateLineRenderer()
        {
            if(_graphPath.Count == 0)
                return;

            int size = _graphPath.Count;

            if (!_navigationCompleted)
            {
                size++; 
            }
            
            Vector3[] array = new Vector3[size];
            for (var i = 0; i < _graphPath.Count; i++)
            {
                var node = _graphPath[i];
                array[i] = node.Position;
            }
            
            if (!_navigationCompleted)
            {
                array[^1] = _edgePosition.Position;
            }

            _lineRenderer.positionCount = array.Length;
            _lineRenderer.SetPositions(array);
        }

        public void SetBuilding(Building building)
        {
            _building = building;
        }

        [Serializable]
        public class Route
        {
            public GraphNode GraphNode;
            public GraphSubNode GraphSubNode;

            public Vector3 Position => GraphSubNode.transform.position;

            public Route(GraphNode graphNode)
            {
                GraphNode = graphNode;
                GraphSubNode = graphNode.GetAvailablePosition();
                GraphSubNode.Fill();
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            protected bool Equals(Route other)
            {
                return Equals(GraphNode) && Equals(GraphSubNode);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(GraphNode, GraphSubNode);
            }
        }
    }
}