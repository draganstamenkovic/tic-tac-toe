using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;

namespace AI
{
    public class MinimaxAIPlayer : IPlayer
    {
        public UniTask<Vector2Int> MakeMoveAsync(BoardModel board)
        {
            var move = FindBestMove(board);
            return UniTask.FromResult(move);
        }

        private Vector2Int FindBestMove(BoardModel board)
        {
            int bestScore = int.MinValue;
            Vector2Int bestMove = Vector2Int.zero;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board.GetCell(x, y) == CellState.Empty)
                    {
                        board.PlaceMark(x, y, CellState.O);
                        int score = Minimax(board, 0, false);
                        board.RemoveMark(x, y);

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = new Vector2Int(x, y);
                        }
                    }
                }
            }

            return bestMove;
        }

        private int Minimax(BoardModel board, int depth, bool isMaximizing)
        {
            var state = board.Evaluate();

            if (state == GameState.OWon) return 10 - depth;
            if (state == GameState.XWon) return depth - 10;
            if (state == GameState.Draw) return 0;

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (board.GetCell(x, y) == CellState.Empty)
                        {
                            board.PlaceMark(x, y, CellState.O);
                            int score = Minimax(board, depth + 1, false);
                            board.RemoveMark(x, y);
                            bestScore = Mathf.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (board.GetCell(x, y) == CellState.Empty)
                        {
                            board.PlaceMark(x, y, CellState.X);
                            int score = Minimax(board, depth + 1, true);
                            board.RemoveMark(x, y);
                            bestScore = Mathf.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }
    }
}

