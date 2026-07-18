using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;

namespace AI
{
    public interface IPlayer
    {
        UniTask<Vector2Int> MakeMoveAsync(BoardModel board);
    }
}