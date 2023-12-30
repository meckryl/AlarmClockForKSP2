using System;
using System.Collections.Generic;
using System.Text;

namespace AlarmClockForKSP2
{
    public struct AlarmPersistentData
    {
        public AlarmPersistentData(string name, FormattedTime time) 
        {
            Name = name;
            Time = time;
        }
        public string Name { get; }
        public FormattedTime Time { get; }
    }
    public class Alarm : IComparable<Alarm>
    {
        private string _name;
        private FormattedTimeWrapper _targetTime;

        public Alarm(string name, double time)
        {
            _name = name;
            _targetTime = new FormattedTimeWrapper(time);
        }

        public Alarm(string name, FormattedTimeWrapper time)
        {
            _name = name;
            _targetTime = time;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public FormattedTimeWrapper Time
        {
            get => _targetTime;
        }

        public double TimeAsSeconds
        {
            get => _targetTime.asSeconds();
        }

        public AlarmPersistentData CreatePersistentData()
        {
            return new AlarmPersistentData(_name, _targetTime.time);
        }

        public int CompareTo(Alarm other)
        {
            if (other == null) return 1;

            return TimeAsSeconds.CompareTo(other.TimeAsSeconds);
        }

        public static bool operator > (Alarm a, Alarm b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator < (Alarm a, Alarm b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >= (Alarm a, Alarm b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <= (Alarm a, Alarm b)
        {
            return a.CompareTo(b) <= 0;
        }
    }
}
