using KSP.Game;

namespace AlarmClockForKSP2
{
    public static class GameStateManager
    {
        public static GameStateConfiguration GameState;

        private static int[] _invalidStates = {
            (int)KSP.Game.GameState.Flag,
            (int)KSP.Game.GameState.MainMenu,
            (int)KSP.Game.GameState.Loading,
            (int)KSP.Game.GameState.WarmUpLoading,
            (int)KSP.Game.GameState.Invalid
        };

        public static void UpdateGameState()
        {
            GameState = GameManager.Instance?.Game?.GlobalGameState?.GetGameState();

            if (GameState != null )
            {
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Game state updated to {GameState.GameState}");
            }
        }

        public static bool IsGameStateValid()
        {
            UpdateGameState();
            if (GameState == null)
            {
                return false;
            }

            foreach (int state in _invalidStates)
            {
                if ((int)GameState.GameState == state)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
