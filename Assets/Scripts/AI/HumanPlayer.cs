using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;

namespace AI
{
    public class HumanPlayer : IPlayer
    {
        private Vector2Int _selectedMove = Vector2Int.zero;
        private bool _moveReceived;

        public UniTask<Vector2Int> MakeMoveAsync(BoardModel board)
        {
            _moveReceived = false;
            _selectedMove = Vector2Int.zero;
            
            return WaitForMoveAsync();
        }

        private async UniTask<Vector2Int> WaitForMoveAsync()
        {
            while (!_moveReceived)
            {
                await UniTask.Delay(100);
            }
            return _selectedMove;
        }

        public void ReceiveMove(int x, int y)
        {
            _selectedMove = new Vector2Int(x, y);
            _moveReceived = true;
        }
    }
}
