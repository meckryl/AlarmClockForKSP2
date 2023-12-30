using KSP.Messages;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class AlarmsListController
    {
        private WindowController _parentController;

        private bool _isVisible = false;

        private VisualElement _alarmsListContainer;

        public Button NewAlarmButton;
        public ListView AlarmsListView;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                _alarmsListContainer.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public AlarmsListController(WindowController parentController, VisualElement alarmsListContainer)
        {
            _parentController = parentController;
            _alarmsListContainer = alarmsListContainer;

            if (_alarmsListContainer != null)
            {
                NewAlarmButton = _alarmsListContainer.Q<Button>("new-alarm-button");
                NewAlarmButton.clicked += NewAlarmButtonClicked;

                List<Alarm> alarms = TimeManager.Instance.alarms;

                Func<VisualElement> makeItem = () =>
                {
                    AlarmVisualElement alarmVisualElement = new AlarmVisualElement();

                    return alarmVisualElement;
                };

                Action<VisualElement, int> bindItem = (e, i) => BindItem(e as AlarmVisualElement, i);

                int itemHeight = 40;

                AlarmsListView = new ListView(TimeManager.Instance.alarms, itemHeight, makeItem, bindItem);
                AlarmsListView.reorderable = false;

                _alarmsListContainer.Add(AlarmsListView);

                PersistentDataManager.RegisterAlarmReset(ResetAlarms);
                MessageManager.MessageCenter.PersistentSubscribe<QuitToMainMenuStartedMessage>(_ => ResetAlarms());
            }
            else
            {
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogError("Alarms List Container was null");
            }
        }

        private void NewAlarmButtonClicked()
        {
            _parentController.RefreshVisibility(1);
        }

        private void BindItem(AlarmVisualElement elem, int index)
        {
            if (elem.Q<Label>("name") is Label nameLabel)
            {
                nameLabel.text = TimeManager.Instance.alarms[index].Name;
            }
            if (elem.Q<Label>("time") is Label timeLabel)
            {
                timeLabel.text = TimeManager.Instance.alarms[index].Time.asString();
            }
            if (elem.Q<Button>("close") is Button closeButton)
            {
                closeButton.RegisterCallback<ClickEvent>(_ => {
                    TimeManager.Instance.alarms.RemoveAt(index);
                    AlarmsListView.Rebuild();
                });
                ;
            }

        }

        private bool ResetAlarms()
        {
            TimeManager.Instance.alarms = new List<Alarm>();
            AlarmsListView.itemsSource = TimeManager.Instance.alarms;
            AlarmsListView.Rebuild();
            return true;
        }
    }
}
