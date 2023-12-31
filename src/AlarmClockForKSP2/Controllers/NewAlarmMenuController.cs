﻿using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class NewAlarmMenuController : VisualElement
    {
        private WindowController _parentController;

        private bool _isVisible = false;

        public Button ManueverButton;
        public Button SoiButton;
        public Button TransferWindowButton;
        public Button CustomAlarmButton;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public NewAlarmMenuController(WindowController parentController)
        {
            _parentController = parentController;
            TemplateContainer root = AssetManager.GetAsset<VisualTreeAsset>($"alarmclockforksp2/" + "alarmclock-resources/UI/NewAlarmMenu.uxml").CloneTree();

            if (root != null)
            {
                Add(root);
                ManueverButton = this.Q<Button>("manuever-button");
                ManueverButton.clicked += DefaultToListView;

                SoiButton = this.Q<Button>("soi-button");
                SoiButton.clicked += DefaultToListView;

                TransferWindowButton = this.Q<Button>("transfer-window-button");
                TransferWindowButton.clicked += TransferWindowButtonClicked;

                CustomAlarmButton = this.Q<Button>("custom-button");
                CustomAlarmButton.clicked += CustomAlarmButtonClicked;
            }
            else
            {
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogError("New Alarm Container was null");
            }
        }
        private void DefaultToListView()
        {
            _parentController.RefreshVisibility(0);
        }

        private void TransferWindowButtonClicked()
        {
            _parentController.RefreshVisibility(2);
        }

        private void CustomAlarmButtonClicked()
        {
            _parentController.RefreshVisibility(3);
        }
    }
}
