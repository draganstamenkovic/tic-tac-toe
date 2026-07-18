using System;
using Model;
using UnityEngine;

namespace View
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Renderer cellRenderer;
        [SerializeField] private Material emptyMaterial;
        [SerializeField] private Material xMaterial;
        [SerializeField] private Material oMaterial;

        public int X { get; private set; }
        public int Y { get; private set; }

        public event Action<int, int> CellClicked;

        private BoardModel _board;

        public void Initialize(int x, int y, BoardModel board)
        {
            X = x;
            Y = y;
            _board = board;
            SetVisuals(CellState.Empty);
        }

        public void SetVisuals(CellState state)
        {
            if (cellRenderer == null)
                cellRenderer = GetComponent<Renderer>();

            var mat = state switch
            {
                CellState.X => xMaterial,
                CellState.O => oMaterial,
                _ => emptyMaterial
            };

            cellRenderer.material = mat;
        }

        private void OnMouseDown()
        {
            var cellState = _board?.GetCell(X, Y);
            
            if (cellState == CellState.Empty)
                CellClicked?.Invoke(X, Y);
        }
    }
}