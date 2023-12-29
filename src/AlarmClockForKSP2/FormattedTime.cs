using KSP.UI.Binding;
using Shapes;
using VehiclePhysics;

namespace AlarmClockForKSP2
{
    public struct FormattedTime
    {
        public double Year;
        public double Day;
        public double Hour;
        public double Minute;
        public double Second;
    }
    public class FormattedTimeWrapper
    {
        private FormattedTime _time;
        public FormattedTime time { get { return _time; } }

        public static readonly int DaysInYear = 425;
        public static readonly int HoursInDay = 6;

        public FormattedTimeWrapper(double year, double day, double hour, double minute, double second)
        {
            _time.Year = year;
            _time.Day = day;
            _time.Hour = hour;
            _time.Minute = minute;
            _time.Second = second;
        }

        public FormattedTimeWrapper(double timeSeconds)
        {
            UIValue_ReadNumber_DateTime.DateTime internalTime = UIValue_ReadNumber_DateTime.ComputeDateTime(timeSeconds, 6, 425);

            _time.Year = internalTime.Years;
            _time.Day = internalTime.Days;
            _time.Hour = internalTime.Hours;
            _time.Minute = internalTime.Minutes;
            _time.Second = internalTime.Seconds;
        }

        public string asString()
        {
            return "Year: " + $"{_time.Year + 1} " +
                "Day: " + $"{_time.Day + 1} " +
                $"{_time.Hour:00}:{_time.Minute:00}:{_time.Second:00}";
        }
        public double asSeconds()
        {
            return _time.Year * DaysInYear * HoursInDay * 3600
                + _time.Day * HoursInDay * 3600
                + _time.Hour * 3600
                + _time.Minute * 60
                + _time.Second;
        }

    }
}
