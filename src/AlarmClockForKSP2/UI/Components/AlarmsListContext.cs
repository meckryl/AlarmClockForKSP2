using KSP.Messages;
using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class AlarmsListContext : ContextElement
    {
        public Button NewAlarmButton;
        public ListView AlarmsListView;

        public Button SettingsButton;


        public AlarmsListContext(Action<int> swapContext) : base(swapContext, "alarmclock-resources/UI/AlarmsList.uxml")
        {
            NewAlarmButton = this.Q<Button>("new-alarm-button");
            NewAlarmButton.clicked += NewAlarmButtonClicked;

            List<Alarm> alarms = TimeManager.Instance.alarms;

            Func<VisualElement> makeItem = () =>
            {
                AlarmVisualElement alarmVisualElement = new AlarmVisualElement();

                return alarmVisualElement;
            };

            Action<VisualElement, int> bindItem = (e, i) => BindItem(e as AlarmVisualElement, i);

            int itemHeight = 46;

            AlarmsListView = this.Q<ListView>("alarms-listview");
            AlarmsListView.itemsSource = TimeManager.Instance.alarms;
            AlarmsListView.fixedItemHeight = itemHeight;
            AlarmsListView.makeItem = makeItem;
            AlarmsListView.bindItem = bindItem;

            AlarmsListView.reorderable = false;

            //Add(AlarmsListView);

            PersistentDataManager.RegisterAlarmReset(ResetAlarms);
            MessageManager.MessageCenter.PersistentSubscribe<QuitToMainMenuStartedMessage>(_ => ResetAlarms());

            SettingsButton = this.Q<Button>("options-button");
            SettingsButton.clicked += SettingsClicked;

        }

        private void NewAlarmButtonClicked()
        {
            _swapContext((int)MainWindowContext.NewAlarm);
        }

        private void BindItem(AlarmVisualElement elem, int index)
        {
            if (elem.Q<Label>("name") is Label nameLabel)
            {
                nameLabel.text = TimeManager.Instance.alarms[index].Name;
            }
            if (elem.Q<Label>("time") is Label timeLabel)
            {
                timeLabel.text = TimeManager.Instance.alarms[index].Time.asShortString();
            }
            if (elem.Q<Button>("close") is Button closeButton)
            {
                closeButton.RegisterCallback<ClickEvent>(_ =>
                {
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

        public void RebuildAlarmList()
        {
            AlarmsListView.Rebuild();

        }

        private void SettingsClicked()
        {
            _swapContext((int)MainWindowContext.Settings);
        }
    }
}
