using System;
using System.Collections.Generic;
using System.Linq;
using geniikw.DataRenderer2D;
using InputHandling;
using Navigation;
using UI;
using UnityEngine;

namespace Building
{
    public class BuildingRoute : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private Graph _graph;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private RouteInventory _routeInventory;
        [SerializeField] private WorldLine _worldLine;
        [SerializeField] private SpriteRenderer _pointColor;
        [SerializeField] private float _size = 1f;

        private bool _navigationCompleted = false;
        private List<Route> _graphPath = new List<Route>();
        private EdgePosition _edgePosition;
        private GraphNode _lastNode;
        private bool _selected = false;
        private Color _color;

        public event Action<BuildingRoute> NewRouteCreated;
        public event Action<BuildingRoute> RouteReseted;
        public event Action<BuildingRoute> ColorUpdated;
        
        public bool IsActive => _navigationCompleted;
        public bool Available => _building == null;
        public Color Color => _color;
        private int _index = 0;

        private void Start()
        {
            _routeInventory.AddRoute(this);
            _worldLine.MakeNewMesh();
            _worldLine.gameObject.SetActive(true);
            _lineRenderer.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _routeInventory.RemoveRoute(this);
        }

        public void SetColor(Color color)
        {
            _color = color;
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                    { new GradientColorKey(_color, 0.0f), new GradientColorKey(_color, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) });
            _lineRenderer.colorGradient = gradient;
            _worldLine.line.option.color = gradient;
            _pointColor.color = color;
            ColorUpdated?.Invoke(this);
        }

        public void SelectPath()
        {
            _selected = true;
        }

        public void ResetPath()
        {
            _navigationCompleted = false;
            
            _graphPath.Clear();
            _lineRenderer.positionCount = 0;
            _worldLine.line.Clear();
            RouteReseted?.Invoke(this);
            _worldLine.gameObject.SetActive(true);
            _lineRenderer.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_selected)
            {
                if (Input.GetMouseButton(0) && !_navigationCompleted)
                {
                    if (_graphPath.Count == 0)
                    {
                        _graphPath.Add(new Route(_building.Node, _index));
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
                                _graphPath.RemoveAt(i);
                                _lastNode = _graphPath[^1].GraphNode;
                            }
                            else if (_lastNode.SiblingNodes.Contains(graphNode) && (_graph.IsPositionInsideNode(mousePosition, graphNode) || !_edgePosition.Contains(_lastNode)))
                            {
                                _graphPath.Add(new Route(graphNode, _index));
                                _lastNode = graphNode;
                            }
                        }
                    }

                    if (graphNode == _building.Node && _graphPath.Count > 1 &&
                        _lastNode.SiblingNodes.Contains(_building.Node))
                    {
                        _navigationCompleted = true;
                        _graphPath.Add(new Route(_building.Node, _index));
                        NewRouteCreated?.Invoke(this);
                        _inputHandler.RouteCreated();
                        _selected = false;
                        _worldLine.gameObject.SetActive(false);
                        _lineRenderer.gameObject.SetActive(true);
                        _building.RouteFinished();
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

            int morePoints = _worldLine.line.Count - size;
            
            for (int i = 0; i < morePoints; i++)
            {
                _worldLine.line.Pop();
            }
    
            while (_worldLine.line.Count < size)
            {
                _worldLine.line.Push();
            }
    
            for (int i = 0; i < _worldLine.line.Count; i++)
            {
                _worldLine.line.EditPoint(i, array[i], _size);
            }
            
            _lineRenderer.positionCount = array.Length;
            _lineRenderer.SetPositions(array);
            transform.position = array[0];
        }

        public void SetBuilding(Building building)
        {
            _building = building;
            _index = _building.Node.GetAvailableIndex();
            GraphSubNode graphSubNode = _building.Node.GetPosition(_index);
            graphSubNode.Fill();
        }

        public List<Vector3> GetPath()
        {
            return _graphPath.Select(x => x.Position).ToList();
        }

        public void RemoveRoute()
        {
            if (_building != null)
            {
                GraphSubNode graphSubNode = _building.Node.GetPosition(_index);
                graphSubNode.Empty();
            }
            _routeInventory.ReturnRoute(this);
            ResetPath();
            if (_building != null)
            {
                _building.DestroyRoute(this);
                _building = null;
            }
        }

        [Serializable]
        public class Route
        {
            public GraphNode GraphNode;
            public GraphSubNode GraphSubNode;

            public Vector3 Position => GraphSubNode.transform.position;

            public Route(GraphNode graphNode, int index)
            {
                GraphNode = graphNode;
                GraphSubNode = graphNode.GetPosition(index);
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