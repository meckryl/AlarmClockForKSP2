﻿using KSP.Game;
using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class TransferWindowMenuController : VisualElement
    {
        private WindowController _parentController;

        private bool _isVisible = false;

        public DropdownField TransferFromDropdown;
        public DropdownField TransferToDropdown;
        public Label TransferTimeLabel;
        public Button TransferConfirmButton;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public TransferWindowMenuController(WindowController parentController)
        {
            _parentController = parentController;
            TemplateContainer root = AssetManager.GetAsset<VisualTreeAsset>($"alarmclockforksp2/" + "alarmclock-resources/UI/TransferWindowMenu.uxml").CloneTree();

            if (root != null)
            {
                Add(root);
                TransferFromDropdown = this.Q<DropdownField>("transfer-from-dropdown");
                TransferToDropdown = this.Q<DropdownField>("transfer-to-dropdown");
                TransferTimeLabel = this.Q<Label>("transfer-time-label");

                TransferConfirmButton = this.Q<Button>("transfer-confirm-button");
                TransferConfirmButton.clicked += TransferConfirmButtonClicked;
            }
            else
            {
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogError("Transfer Window Container was null");
            }
        }

        private void TransferConfirmButtonClicked()
        {
            int originIndex = TransferFromDropdown.index;
            int destinationIndex = TransferToDropdown.index;

            double nextWindow = TransferWindowPlanner.getNextTransferWindow(
                TransferWindowPlanner.planets[originIndex],
                TransferWindowPlanner.planets[destinationIndex],
                GameManager.Instance.Game.UniverseModel.UniverseTime);

            TimeManager.Instance.AddAlarm($"{TransferWindowPlanner.planets[originIndex].Name} to {TransferWindowPlanner.planets[destinationIndex].Name}", nextWindow);
            _parentController.AlarmsList.Rebuild();

            _parentController.RefreshVisibility(0);
        }
    }
}
