using UnityEngine;

namespace Model
{
    public class BoardModel
    {
        private const int Size = 3;
        private readonly CellState[,] _board = new CellState[Size, Size];

        public CellState GetCell(int x, int y)
        {
            return _board[x, y];
        }

        public bool PlaceMark(int x, int y, CellState mark)
        {
            if (_board[x, y] != CellState.Empty) return false;
            _board[x, y] = mark;
            return true;
        }

        public void RemoveMark(int x, int y)
        {
            _board[x, y] = CellState.Empty;
        }

        public void ResetBoard()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    _board[x, y] = CellState.Empty;
                }
            }
        }

        public bool IsBoardFull()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (_board[x, y] == CellState.Empty) return false;
                }
            }

            return true;
        }

        public GameState Evaluate()
        {
            // Rows
            for (int y = 0; y < Size; y++)
            {
                if (_board[0, y] != CellState.Empty && _board[0, y] == _board[1, y] && _board[1, y] == _board[2, y])
                {
                    return _board[0, y] == CellState.X ? GameState.XWon : GameState.OWon;
                }
            }

            // Columns
            for (int x = 0; x < Size; x++)
            {
                if (_board[x, 0] != CellState.Empty && _board[x, 0] == _board[x, 1] && _board[x, 1] == _board[x, 2])
                {
                    return _board[x, 0] == CellState.X ? GameState.XWon : GameState.OWon;
                }
            }

            // Diagonals
            if (_board[0, 0] != CellState.Empty && _board[0, 0] == _board[1, 1] && _board[1, 1] == _board[2, 2])
            {
                return _board[0, 0] == CellState.X ? GameState.XWon : GameState.OWon;
            }

            if (_board[2, 0] != CellState.Empty && _board[2, 0] == _board[1, 1] && _board[1, 1] == _board[0, 2])
            {
                return _board[2, 0] == CellState.X ? GameState.XWon : GameState.OWon;
            }

            return IsBoardFull() ? GameState.Draw : GameState.Playing;
        }
    }
}
