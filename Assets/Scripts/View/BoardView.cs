using System;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace View
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private CellView cellPrefab; 
        [SerializeField] private Transform boardContainer;
        [SerializeField] private Renderer renderer;
        [SerializeField] private Material separatorMaterial;            
        [SerializeField] private float separatorThickness = 0.05f;
        [SerializeField] private float separatorHeight = 0.1f;
        
        private float _boardSize;
        private float _boardStart;
        private float _cellSize;
        private readonly List<CellView> _cells = new();
        private BoardModel _board;
        
        public event Action<int, int> CellClicked;

        public void Initialize(BoardModel board)
        {
          //  gameObject.SetActive(true);
            _board = board;
            _boardSize = renderer.bounds.size.x;
            _cellSize = _boardSize / 3;
            _boardStart = _boardSize / -2;
            Build();
            CreateSeparators();
        }

        private void Build()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var cellTransform = boardContainer != null ? boardContainer : transform;
                    
                    var xPos = _boardStart + x * _cellSize + _cellSize / 2;
                    var yPos = _boardStart + y * _cellSize + _cellSize / 2;
                    
                    var position = new Vector3(xPos, 0.05f, yPos);
                    
                    var cell = Instantiate(cellPrefab, position, Quaternion.identity, cellTransform);
                    cell.Initialize(x, y, _board);
                    cell.CellClicked += OnCellClicked;
                    _cells.Add(cell);
                }
            }
        }
        
        private void OnCellClicked(int x, int y)
        {
            CellClicked?.Invoke(x, y);
        }

        private void CreateSeparators()
        {
            var cellTransform = boardContainer != null ? boardContainer : transform;
            var separatorLength = _boardSize - separatorThickness - separatorHeight;
            for (int i = 1; i < 3; i++)
            {
                float xPos = _boardStart + i * _cellSize;
                CreateSeparator(new Vector3(xPos, separatorHeight, 0), 
                               new Vector3(separatorThickness, separatorHeight, separatorLength), cellTransform);
            }

            for (int i = 1; i < 3; i++)
            {
                float zPos = _boardStart + i * _cellSize;
                CreateSeparator(new Vector3(0, separatorHeight, zPos), 
                               new Vector3(separatorLength, separatorHeight, separatorThickness), cellTransform);
            }
        }

        private void CreateSeparator(Vector3 position, Vector3 scale, Transform parent)
        {
            var separatorObj = new GameObject("GridSeparator");
            separatorObj.transform.SetParent(parent);
            separatorObj.transform.localPosition = position;
            separatorObj.transform.localScale = scale;

            var meshFilter = separatorObj.AddComponent<MeshFilter>();
            meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");

            var separatorRenderer = separatorObj.AddComponent<MeshRenderer>();
            if (separatorMaterial != null)
                separatorRenderer.material = separatorMaterial;
            else
            {
                var mat = new Material(Shader.Find("Standard"));
                mat.color = Color.black;
                separatorRenderer.material = mat;
            }
        }

        public void DisplayCellState(int x, int y, CellState state)
        {
            foreach (var cell in _cells)
            {
                if (cell.X == x && cell.Y == y)
                {
                    cell.SetVisuals(state);
                    break;
                }
            }
        }

        public void ClearBoard()
        {
            foreach(var cell in _cells)
            {
                Destroy(cell.gameObject);
            }
            _cells.Clear();
            Build();
            gameObject.SetActive(false);
        }
    }
}