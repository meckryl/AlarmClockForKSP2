using SpaceWarp;
using JetBrains.Annotations;
using KSP.Game;
using KSP.Sim;
using UnityEngine;
using System.Collections.Generic;
using KSP.Sim.impl;

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
                double timeDelta = (um.UniverseTime - _previousTime) / tw.CurrentRate;

                if (um.UniverseTime + tw.CurrentRate*timeDelta*0.5>= alarms[0].TimeAsSeconds)
                {
                    AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage("Alarm!!");

                    HandleTimeStop(timeDelta, um, tw);
                }
            }
            
        }

        private void HandleTimeStop(double timeDelta, UniverseModel um, TimeWarp tw)
        {
            if (tw.IsAutoWarpEngaged)
            {
                tw.CancelAutoWarp();
            }

            if (alarms[0].TimeAsSeconds - um.UniverseTime < 10)
            {
                tw.SetRateIndex(0, true);
                alarms.Remove(alarms[0]);
                tw.SetIsPaused(true);
                um.SetTimePaused(true);
            }
            else
            {
                int safeRate = (int)Math.Log10(alarms[0].TimeAsSeconds - um.UniverseTime) + 4;
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
            //todo: if um is null, pass and return false
            double universetime = um.UniverseTime;

            if (time.asSeconds() <= universetime) return;

            Alarm alarm = new Alarm(name, time);
            Predicate<Alarm> query = delegate (Alarm existingAlarm) { return alarm >= existingAlarm; };
            int index = alarms.FindLastIndex(query) + 1;
            alarms.Insert(index, alarm);
            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage("Created " + $"{alarms.Count}");
        }

        public static TimeManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TimeManager();
                return _instance;
            }
        }
    }
}
