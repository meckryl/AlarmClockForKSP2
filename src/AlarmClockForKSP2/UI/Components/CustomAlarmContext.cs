using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class CustomAlarmContext : ContextElement
    {
        private TextField _nameTextField;
        private IntegerField _yearIntegerField;
        private IntegerField _dayIntegerField;
        private IntegerField _hourIntegerField;
        private IntegerField _minuteIntegerField;
        private IntegerField _secondIntegerField;
        private Button _customConfirmButton;

        public CustomAlarmContext(Action<int> swapContext) : base(swapContext, "alarmclock-resources/UI/CustomAlarmMenu.uxml")
        {

            _nameTextField = this.Q<TextField>("name-textfield");
            _yearIntegerField = this.Q<IntegerField>("year-integerfield");
            _dayIntegerField = this.Q<IntegerField>("day-integerfield");
            _hourIntegerField = this.Q<IntegerField>("hour-integerfield");
            _minuteIntegerField = this.Q<IntegerField>("minute-integerfield");
            _secondIntegerField = this.Q<IntegerField>("second-integerfield");
            _customConfirmButton = this.Q<Button>("custom-confirm-button");

            _customConfirmButton.clicked += CustomConfirmButtonClicked;
        }

        private void CustomConfirmButtonClicked()
        {
            FormattedTimeWrapper time = new FormattedTimeWrapper(
                _yearIntegerField.value - 1,
                _dayIntegerField.value - 1,
                _hourIntegerField.value,
                _minuteIntegerField.value,
                _secondIntegerField.value
                );

            TimeManager.Instance.AddAlarm(_nameTextField.value, time);
            AlarmClockForKSP2Plugin.Instance.AlarmWindowController.AlarmsList.Rebuild();

            _yearIntegerField.value = 1;
            _dayIntegerField.value = 1;
            _hourIntegerField.value = 0;
            _minuteIntegerField.value = 0;
            _secondIntegerField.value = 0;

            _swapContext((int)MainWindowContext.AlarmsList);
        }
    }
}
