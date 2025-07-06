using SWEN_Game._Managers;

namespace SWEN_Game._Interfaces
{
    public interface IGameStateManager
    {
        public GameState CurrentGameState { get; set; }
        void ChangeGameState(GameState newGameState);
        void CaptureLastFrame();
        void ResetGame();
    }
}
