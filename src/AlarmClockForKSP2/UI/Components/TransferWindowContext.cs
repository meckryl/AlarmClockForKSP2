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

        public Button SettingsButton;


        private Func<SettingsInfo> _getSettings;

        public TransferWindowContext(Action<int> swapContext, Func<SettingsInfo> getSettings) : base(swapContext, "alarmclock-resources/UI/TransferWindowMenu.uxml")
        {
            TransferFromDropdown = this.Q<DropdownField>("transfer-from-dropdown");
            TransferToDropdown = this.Q<DropdownField>("transfer-to-dropdown");
            TransferTimeLabel = this.Q<Label>("transfer-time-label");
            TransferConfirmButton = this.Q<Button>("transfer-confirm-button");

            _getSettings = getSettings;

            TransferConfirmButton.clicked += TransferConfirmButtonClicked;

            SettingsButton = this.Q<Button>("options-button");
            SettingsButton.clicked += SettingsClicked;
        }

        private void TransferConfirmButtonClicked()
        {
            string origin = TransferFromDropdown.value;
            string destination = TransferToDropdown.value;

            double nextWindow = TransferWindowPlanner.getNextTransferWindow(
                origin,
                destination,
                GameManager.Instance.Game.UniverseModel.UniverseTime);

            SettingsInfo settings = _getSettings();
            FormattedTimeWrapper offset = new FormattedTimeWrapper(0, settings.day, settings.hour, settings.minute, settings.second);

            TimeManager.Instance.AddAlarm($"{origin} to {destination}", nextWindow-offset.asSeconds());
            AlarmClockForKSP2Plugin.Instance.AlarmWindowController.AlarmsList.Rebuild();

            _swapContext((int)MainWindowContext.AlarmsList);
        }

        private void SettingsClicked()
        {
            _swapContext((int)MainWindowContext.Settings);
        }
    }
}
