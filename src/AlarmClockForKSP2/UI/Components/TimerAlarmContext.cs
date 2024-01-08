using SpaceWarp.API.Assets;
using UnityEngine.UIElements;
using KSP.Game;

namespace AlarmClockForKSP2
{
    public class TimerAlarmContext : ContextElement
    {
        private TextField _nameTextField;
        private IntegerField _yearIntegerField;
        private IntegerField _dayIntegerField;
        private IntegerField _hourIntegerField;
        private IntegerField _minuteIntegerField;
        private IntegerField _secondIntegerField;
        private Button _customConfirmButton;

        public Button SettingsButton;


        public TimerAlarmContext(Action<int> swapContext) : base(swapContext, "alarmclock-resources/UI/TimerAlarmWindow.uxml")
        {

            _nameTextField = this.Q<TextField>("name-textfield");

            _yearIntegerField = this.Q<IntegerField>("year-integerfield");
            _yearIntegerField.value = 0;

            _dayIntegerField = this.Q<IntegerField>("day-integerfield");
            _dayIntegerField.value = 0;

            _hourIntegerField = this.Q<IntegerField>("hour-integerfield");
            _minuteIntegerField = this.Q<IntegerField>("minute-integerfield");
            _secondIntegerField = this.Q<IntegerField>("second-integerfield");
            _customConfirmButton = this.Q<Button>("custom-confirm-button");

            _customConfirmButton.clicked += CustomConfirmButtonClicked;

            SettingsButton = this.Q<Button>("options-button");
            SettingsButton.clicked += SettingsClicked;
        }

        private void CustomConfirmButtonClicked()
        {
            FormattedTimeWrapper deltaTime = new FormattedTimeWrapper(
                _yearIntegerField.value,
                _dayIntegerField.value,
                _hourIntegerField.value,
                _minuteIntegerField.value,
                _secondIntegerField.value
                );

            FormattedTimeWrapper time = new FormattedTimeWrapper(GameManager.Instance.Game.UniverseModel.UniverseTime + deltaTime.asSeconds());

            TimeManager.Instance.AddAlarm(_nameTextField.value, time);
            AlarmClockForKSP2Plugin.Instance.AlarmWindowController.AlarmsList.Rebuild();

            _yearIntegerField.value = 0;
            _dayIntegerField.value = 0;
            _hourIntegerField.value = 0;
            _minuteIntegerField.value = 0;
            _secondIntegerField.value = 0;

            _swapContext((int)MainWindowContext.AlarmsList);
        }

        private void SettingsClicked()
        {
            _swapContext((int)MainWindowContext.Settings);
        }
    }
}
