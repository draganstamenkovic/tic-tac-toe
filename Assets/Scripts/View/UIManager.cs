using AI;
using Controller;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private Button humanVsHumanButton;
        [SerializeField] private Button humanVsAIButton;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Button restartButton;
        [SerializeField] private TimeView timeView;

        private GameController _gameController;
        private TurnController _turnController;

        public void Initialize(TurnController turnController, GameController gameController)
        {
            _gameController = gameController;
            _turnController = turnController;
            humanVsHumanButton.onClick.AddListener(() => OnModeSelected(true));
            humanVsAIButton.onClick.AddListener(() => OnModeSelected(false));
            restartButton.onClick.AddListener(OnRestart);

            gameOverPanel.SetActive(false);
            ShowModeSelection();
        }

        private void ShowModeSelection()
        {
            mainMenuPanel.SetActive(true);
        }

        private void OnModeSelected(bool isHumanVsHuman)
        {
            mainMenuPanel.SetActive(false);
            StartGame(isHumanVsHuman);
        }

        private async void StartGame(bool isHumanVsHuman)
        {
            var player1 = new HumanPlayer();
            IPlayer player2;

            if (isHumanVsHuman)
                player2 = new HumanPlayer();
            else
                player2 = new RandomAIPlayer();
            timeView.ShowTime();
            await _turnController.InitializeGameAsync(player1, player2, CellState.X);
        }

        public void ShowGameOver(GameState state)
        {
            timeView.HideTime();
            var message = state switch
            {
                GameState.XWon => "Player X Won!",
                GameState.OWon => "Player O Won!",
                GameState.Draw => "It's a Draw!",
                _ => "Game Over"
            };

            gameOverText.text = message;
            gameOverPanel.SetActive(true);
        }

        private void OnRestart()
        {
            gameOverPanel.SetActive(false);
            _gameController.ResetGame();
            ShowModeSelection();
        }
    }
}