using KSP.Game;
using KSP.Messages;
using System.ComponentModel;

namespace AlarmClockForKSP2
{
    public static class LoadUtilities
    {
        public static MessageCenter MessageCenter;

        public static void InitializeGameManager()
        {
            MessageCenter = GameManager.Instance?.Game?.Messages;
        }
    }
}
