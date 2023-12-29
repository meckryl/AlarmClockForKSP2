using KSP.Game;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class TransferWindowMenuController
    {
        private WindowController _parentController;

        private bool _isVisible = false;

        private VisualElement _transferWindowContainer;

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
                _transferWindowContainer.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public TransferWindowMenuController(WindowController parentController, VisualElement transferWindowContainer)
        {
            _parentController = parentController;
            _transferWindowContainer = transferWindowContainer;

            if (_transferWindowContainer != null)
            {
                TransferFromDropdown = _transferWindowContainer.Q<DropdownField>("transfer-from-dropdown");
                TransferToDropdown = _transferWindowContainer.Q<DropdownField>("transfer-to-dropdown");
                TransferTimeLabel = _transferWindowContainer.Q<Label>("transfer-time-label");

                TransferConfirmButton = _transferWindowContainer.Q<Button>("transfer-confirm-button");
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
