using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class CustomAlarmMenuController
    {
        private WindowController _parentController;

        private bool _isVisible = false;

        private VisualElement _customAlarmContainer;

        public TextField NameTextField;
        public IntegerField YearIntegerField;
        public IntegerField DayIntegerField;
        public IntegerField HourIntegerField;
        public IntegerField MinuteIntegerField;
        public IntegerField SecondIntegerField;
        public Button CustomConfirmButton;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                _customAlarmContainer.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public CustomAlarmMenuController(WindowController parentController, VisualElement customAlarmContainer)
        {
            _parentController = parentController;
            _customAlarmContainer = customAlarmContainer;

            if (_customAlarmContainer != null)
            {

                NameTextField = _customAlarmContainer.Q<TextField>("name-textfield");
                YearIntegerField = _customAlarmContainer.Q<IntegerField>("year-integerfield");
                DayIntegerField = _customAlarmContainer.Q<IntegerField>("day-integerfield");
                HourIntegerField = _customAlarmContainer.Q<IntegerField>("hour-integerfield");
                MinuteIntegerField = _customAlarmContainer.Q<IntegerField>("minute-integerfield");
                SecondIntegerField = _customAlarmContainer.Q<IntegerField>("second-integerfield");
                CustomConfirmButton = _customAlarmContainer.Q<Button>("custom-confirm-button");

                CustomConfirmButton.clicked += CustomConfirmButtonClicked;
            }
            else
            {
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogError("Custom Alarm Container was null");
            }
        }

        private void CustomConfirmButtonClicked()
        {
            FormattedTimeWrapper time = new FormattedTimeWrapper(
                YearIntegerField.value - 1,
                DayIntegerField.value - 1,
                HourIntegerField.value,
                MinuteIntegerField.value,
                SecondIntegerField.value
                );

            TimeManager.Instance.AddAlarm(NameTextField.value, time);
            _parentController.AlarmsList.Rebuild();

            YearIntegerField.value = 1;
            DayIntegerField.value = 1;
            HourIntegerField.value = 0;
            MinuteIntegerField.value = 0;
            SecondIntegerField.value = 0;

            _parentController.RefreshVisibility(0);
        }
    }
}
