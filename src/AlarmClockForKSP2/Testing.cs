using KSP.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlarmClockForKSP2
{
    public static class Testing
    {
        public static void SubscribeToMessages()
        {
            MessageManager.InitializeMessageCenter();

            MessageManager.MessageCenter.PersistentSubscribe<GameLoadFinishedMessage>(CreateAlarms);
        }
        public static void CreateAlarms(MessageCenterMessage obj)
        {
            TimeManager.Instance.AddAlarm("a", 500);
            TimeManager.Instance.AddAlarm("b", 700);
            TimeManager.Instance.AddAlarm("c", 10000);
            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage("Alarms Created!");
        }
    }
}
