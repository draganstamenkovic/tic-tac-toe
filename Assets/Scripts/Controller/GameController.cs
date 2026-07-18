using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private TurnController turnController;
        
        private BoardModel _board;

        public void Start()
        {
            _board = new BoardModel();
            boardView.Initialize(_board);
            uiManager.Initialize(turnController, this);
        }

        public void DisplayMove(int x, int y, CellState player)
        {
            boardView.DisplayCellState(x, y, player);
        }

        public void EndGame(GameState state)
        {
            uiManager.ShowGameOver(state);
            turnController.StopGame();
        }

        public BoardModel GetBoard()
        {
            return _board;
        }

        public void ResetGame()
        {
            _board.ResetBoard();
            boardView.ClearBoard();
        }
    }
}
