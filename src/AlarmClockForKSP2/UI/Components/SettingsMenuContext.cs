using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public struct SettingsInfo
    {
        public int transferCalcMethod;
        public int day;
        public int hour;
        public int minute;
        public int second;

        public SettingsInfo(int transferCalcMethod, int day, int hour, int minute, int second)
        {
            this.transferCalcMethod = transferCalcMethod;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }
    }
    public class SettingsMenuContext : ContextElement
    {
        private IntegerField _dayIntegerField;
        private IntegerField _hourIntegerField;
        private IntegerField _minuteIntegerField;
        private IntegerField _secondIntegerField;

        private DropdownField _transferCalcMethod;
        private Button _closeSettings;

        public SettingsMenuContext(Action<int> swapContext) : base(swapContext, "alarmclock-resources/UI/SettingsMenu.uxml")
        {
            _dayIntegerField = this.Q<IntegerField>("day-integerfield");
            _hourIntegerField = this.Q<IntegerField>("hour-integerfield");
            _minuteIntegerField = this.Q<IntegerField>("minute-integerfield");
            _secondIntegerField = this.Q<IntegerField>("second-integerfield");

            _transferCalcMethod = this.Q<DropdownField>("transfer_method_dropdown");

            _closeSettings = this.Q<Button>("close-options-button");
            _closeSettings.clicked += CloseSettingsClicked;
        }

        private void CloseSettingsClicked()
        {
            _swapContext((int)AlarmClockForKSP2Plugin.Instance.AlarmWindowController.PreviousState);
        }

        public SettingsInfo GetSettings()
        {
            return new SettingsInfo(
                _transferCalcMethod.index,
                _dayIntegerField.value,
                _hourIntegerField.value,
                _minuteIntegerField.value,
                _secondIntegerField.value
            );
        }
    }
}
