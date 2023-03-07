using UnityEngine;

namespace TonyLearning.ShootingGame.System_Modules
{
    public class GameManager : Singleton<GameManager>
    {
        public static System.Action onGameOver;
        public static GameState GameState
        {
            get => Instance._gameState;
            set => Instance._gameState = value;
        }
        [SerializeField]private GameState _gameState = GameState.Playing;
    }

    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        Scoring,
        Options,
    }
}
