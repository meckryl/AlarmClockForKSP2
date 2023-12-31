using KSP.Game;
using KSP.Messages;
using System.ComponentModel;

namespace AlarmClockForKSP2
{
    public static class MessageManager
    {
        public static MessageCenter MessageCenter;

        public static void InitializeMessageCenter()
        {
            MessageCenter = GameManager.Instance?.Game?.Messages;
        }
    }
}
