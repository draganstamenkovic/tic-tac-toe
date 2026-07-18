using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;

namespace AI
{
    public class RandomAIPlayer : IPlayer
    {
        public UniTask<Vector2Int> MakeMoveAsync(BoardModel board)
        {
            List<Vector2Int> available = new();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board.GetCell(x, y) == CellState.Empty)
                    {
                        available.Add(new Vector2Int(x, y));
                    }
                }
            } 
            var move = available[UnityEngine.Random.Range(0, available.Count)]; 
            return UniTask.FromResult(move);
        }
    }
}