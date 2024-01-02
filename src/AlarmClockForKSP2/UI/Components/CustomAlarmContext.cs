using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class CustomAlarmMenuController : VisualElement
    {
        private WindowController _parentController;

        private bool _isVisible = false;

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
                style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public CustomAlarmMenuController(WindowController parentController)
        {
            _parentController = parentController;
            TemplateContainer root = AssetManager.GetAsset<VisualTreeAsset>($"alarmclockforksp2/" + "alarmclock-resources/UI/CustomAlarmMenu.uxml").CloneTree();

            if (root != null)
            {
                Add(root);

                NameTextField = this.Q<TextField>("name-textfield");
                YearIntegerField = this.Q<IntegerField>("year-integerfield");
                DayIntegerField = this.Q<IntegerField>("day-integerfield");
                HourIntegerField = this.Q<IntegerField>("hour-integerfield");
                MinuteIntegerField = this.Q<IntegerField>("minute-integerfield");
                SecondIntegerField = this.Q<IntegerField>("second-integerfield");
                CustomConfirmButton = this.Q<Button>("custom-confirm-button");

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
