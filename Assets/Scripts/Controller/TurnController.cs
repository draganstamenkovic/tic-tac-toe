using AI;
using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private BoardView boardView;
        private BoardModel _board;

        private IPlayer _player1;
        private IPlayer _player2;
        private CellState _currentPlayer;
        private bool _gameActive;

        public async UniTask InitializeGameAsync(IPlayer player1, IPlayer player2, CellState startingPlayer = CellState.X)
        {
            _player1 = player1;
            _player2 = player2;
            _currentPlayer = startingPlayer;
            _gameActive = true;
            _board = gameController.GetBoard();
            
            boardView.gameObject.SetActive(true);
            boardView.CellClicked += OnCellClicked;

            await RunGameLoopAsync();
        }
        
        private void OnCellClicked(int x, int y)
        {
            if (!_gameActive || _board == null) 
                return;
            
            if (_board.PlaceMark(x, y, _currentPlayer))
            {
                gameController.DisplayMove(x, y, _currentPlayer);
                
                var state = _board.Evaluate();
                if (state != GameState.Playing)
                {
                    _gameActive = false;
                    gameController.EndGame(state);
                    return;
                }
                
                _currentPlayer = _currentPlayer == CellState.X ? CellState.O : CellState.X;
            }
        }

        private async UniTask RunGameLoopAsync()
        {
            while (_gameActive)
            {
                var currentPlayerAI = _currentPlayer == CellState.X ? _player1 : _player2;
                
                if (currentPlayerAI is not HumanPlayer)
                {
                    // added delay to make it look more natural
                    await UniTask.Delay(100);
                    var move = await currentPlayerAI.MakeMoveAsync(_board);
                    
                    if (!_board.PlaceMark(move.x, move.y, _currentPlayer))
                        continue;

                    gameController.DisplayMove(move.x, move.y, _currentPlayer);

                    var state = _board.Evaluate();
                    if (state != GameState.Playing)
                    {
                        _gameActive = false;
                        gameController.EndGame(state);
                        return;
                    }

                    _currentPlayer = _currentPlayer == CellState.X ? CellState.O : CellState.X;
                }
                
                await UniTask.NextFrame();
            }
        }

        public void StopGame()
        {
            _gameActive = false;
        }
    }
}
