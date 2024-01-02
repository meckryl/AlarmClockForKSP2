using KSP.Game;
using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class TransferWindowContext : ContextElement
    {
        public DropdownField TransferFromDropdown;
        public DropdownField TransferToDropdown;
        public Label TransferTimeLabel;
        public Button TransferConfirmButton;

        public TransferWindowContext(Action<int> swapContext) : base(swapContext, "alarmclock-resources/UI/TransferWindowMenu.uxml")
        {
            TransferFromDropdown = this.Q<DropdownField>("transfer-from-dropdown");
            TransferToDropdown = this.Q<DropdownField>("transfer-to-dropdown");
            TransferTimeLabel = this.Q<Label>("transfer-time-label");
            TransferConfirmButton = this.Q<Button>("transfer-confirm-button");

            TransferConfirmButton.clicked += TransferConfirmButtonClicked;
        }

        private void TransferConfirmButtonClicked()
        {
            string origin = TransferFromDropdown.value;
            string destination = TransferToDropdown.value;

            double nextWindow = TransferWindowPlanner.getNextTransferWindow(
                origin,
                destination,
                GameManager.Instance.Game.UniverseModel.UniverseTime);

            TimeManager.Instance.AddAlarm($"{origin} to {destination}", nextWindow);
            AlarmClockForKSP2Plugin.Instance.AlarmWindowController.AlarmsList.Rebuild();

            _swapContext((int)MainWindowContext.AlarmsList);
        }
    }
}
