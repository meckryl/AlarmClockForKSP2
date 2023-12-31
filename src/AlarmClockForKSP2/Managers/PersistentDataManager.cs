namespace AlarmClockForKSP2
{
    public class AlarmClockPluginSaveData
    {
        public List<AlarmPersistentData> SavedAlarms;
    }
    public static class PersistentDataManager
    {
        private static Func<bool> ResetAlarms;
        private static AlarmClockPluginSaveData _saveData;
        public static void InititializePersistentDataManager(string modID)
        {
            _saveData = SpaceWarp.API.SaveGameManager.ModSaves.RegisterSaveLoadGameData<AlarmClockPluginSaveData>(modID, OnGameSaved, OnGameLoad);
        }

        private static void OnGameSaved(AlarmClockPluginSaveData saveData)
        {
            List<AlarmPersistentData> newSave= new List<AlarmPersistentData>();

            foreach (Alarm alarm in TimeManager.Instance.alarms)
            {
                AlarmPersistentData alarmData = alarm.CreatePersistentData();
                newSave.Add(alarmData);
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Saved: {alarmData.Name}");
            }

            saveData.SavedAlarms = newSave;

            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"--Saved {newSave.Count} alarms--");
        }

        private static void OnGameLoad(AlarmClockPluginSaveData saveData)
        {
            if (saveData.SavedAlarms == null)
            {
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Save Data Null");
            }

            ResetAlarms();

            foreach (AlarmPersistentData alarmData in saveData.SavedAlarms)
            {
                FormattedTimeWrapper loadedTime = new FormattedTimeWrapper(alarmData.Time);
                TimeManager.Instance.AddAlarm(alarmData.Name, loadedTime);
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Loaded: {alarmData.Name}");
            }

            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Loaded {saveData.SavedAlarms.Count} alarms");
        }

        public static void RegisterAlarmReset(Func<bool> action)
        {
            ResetAlarms = action;
        }
    }
}
