using KSP.Game;
using KSP.Sim.impl;
using KSP.Messages;

namespace AlarmClockForKSP2
{
    public class TimeManager
    {
        private static TimeManager _instance;

        public List<Alarm> alarms = new List<Alarm>();

        private double _previousTime = 0;

        public void Update()
        {
            if (GameManager.Instance is GameManager gm
                && gm.Game is GameInstance gi 
                && gi.UniverseModel is UniverseModel um 
                && gi.ViewController.TimeWarp is TimeWarp tw
                && alarms.Count > 0)
            {
                double timeDelta = um.UniverseTime - _previousTime;

                if (um.UniverseTime + timeDelta*15>= alarms[0].TimeAsSeconds)
                {
                    AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage("Alarm!!");

                    HandleTimeStop(um, tw, timeDelta);
                }

                _previousTime = um.UniverseTime;
            }
            
        }

        private void HandleTimeStop(UniverseModel um, TimeWarp tw, double timeDelta)
        {
            double timeAsSeconds = alarms[0].TimeAsSeconds;
            double secondsToTarget = timeAsSeconds - um.UniverseTime;

            if (tw.IsAutoWarpEngaged && secondsToTarget - timeDelta <= 10)
            {
                tw.CancelAutoWarp();
            }

            if (secondsToTarget < 10)
            {
                AlarmClockForKSP2Plugin.CreateAlert(alarms[0].Name);
                tw.SetRateIndex(0, true);
                alarms.Remove(alarms[0]);
                tw.SetIsPaused(true);
                um.SetTimePaused(true);
                AlarmClockForKSP2Plugin.AlarmWindowController.AlarmsListController.RebuildAlarmList();
            }
            else
            {
                int safeRate = (int)Math.Log10(secondsToTarget) + 4;
                if (safeRate < tw.CurrentRateIndex)
                {
                    tw.SetRateIndex(safeRate, true);
                }

            }
        }

        public void AddAlarm(string name, double time)
        {
            FormattedTimeWrapper formattedTime = new FormattedTimeWrapper(time);

            AddAlarm(name, formattedTime);
        }

        public void AddAlarm(string name, FormattedTimeWrapper time)
        {
            UniverseModel um = GameManager.Instance?.Game?.UniverseModel;

            if (um != null && time.asSeconds() <= um.UniverseTime) return;

            Alarm alarm = new Alarm(name, time);
            Predicate<Alarm> query = delegate (Alarm existingAlarm) { return alarm >= existingAlarm; };
            int index = alarms.FindLastIndex(query) + 1;
            alarms.Insert(index, alarm);
            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage("Created " + $"{alarms.Count}");
        }

        private void ValidateAlarmsOnLoadIn()
        {
            UniverseModel um = GameManager.Instance?.Game?.UniverseModel;
            double universetime = um.UniverseTime;
            while (alarms.Count > 0 && alarms[0].Time.asSeconds() <= universetime)
            {
                alarms.RemoveAt(0);
            }
        }

        public static TimeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TimeManager();
                    MessageManager.MessageCenter.PersistentSubscribe<QuitToMainMenuStartedMessage>(_ => _instance.ValidateAlarmsOnLoadIn());
                }

                return _instance;
            }
        }
    }
}
