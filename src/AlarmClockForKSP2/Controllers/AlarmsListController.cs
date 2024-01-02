﻿using KSP.Messages;
using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class AlarmsListController : VisualElement
    {
        private WindowController _parentController;

        private bool _isVisible = false;

        public Button NewAlarmButton;
        public ListView AlarmsListView;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public AlarmsListController(WindowController parentController)
        {
            _parentController = parentController;
            TemplateContainer root = AssetManager.GetAsset<VisualTreeAsset>($"alarmclockforksp2/" + "alarmclock-resources/UI/AlarmsList.uxml").CloneTree();

            if (root != null)
            {
                Add(root);
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

                Add(AlarmsListView);

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
                timeLabel.text = TimeManager.Instance.alarms[index].Time.asShortString();
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

        public void RebuildAlarmList()
        {
            AlarmsListView.Rebuild();

        }
    }
}
