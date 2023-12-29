using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class NewAlarmMenuController
    {
        private WindowController _parentController;

        private bool _isVisible = false;

        private VisualElement _newAlarmContainer;

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
                _newAlarmContainer.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public NewAlarmMenuController(WindowController parentController, VisualElement newAlarmContainer)
        {
            _newAlarmContainer = newAlarmContainer;
            _parentController = parentController;

            if (_newAlarmContainer != null)
            {
                ManueverButton = _newAlarmContainer.Q<Button>("manuever-button");
                ManueverButton.clicked += DefaultToListView;

                SoiButton = _newAlarmContainer.Q<Button>("soi-button");
                SoiButton.clicked += DefaultToListView;

                TransferWindowButton = _newAlarmContainer.Q<Button>("transfer-window-button");
                TransferWindowButton.clicked += TransferWindowButtonClicked;

                CustomAlarmButton = _newAlarmContainer.Q<Button>("custom-button");
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
