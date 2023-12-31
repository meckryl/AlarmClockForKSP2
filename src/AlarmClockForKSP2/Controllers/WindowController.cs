using KSP.UI.Binding;
using UitkForKsp2.API;
using UnityEngine;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class WindowController : MonoBehaviour
    {
        // The UIDocument component of the window game object
        private UIDocument _window;

        // The elements of the window that we need to access
        private VisualElement _rootElement;

        private VisualElement _alarmsWindow;

        private AlarmsListController _alarmsListController;
        private NewAlarmMenuController _newAlarmMenuController;
        private TransferWindowMenuController _transferWindowMenuController;
        private CustomAlarmMenuController _customAlarmMenuController;

        public ListView AlarmsList;

        // The backing field for the IsWindowOpen property
        private bool _isWindowOpen = false;

        public bool IsWindowOpen
        {
            get => _isWindowOpen;
            set
            {
                _isWindowOpen = value;

                // Set the display style of the root element to show or hide the window
                _rootElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
                RefreshVisibility(0);
                // Alternatively, you can deactivate the window game object to close the window and stop it from updating,
                // which is useful if you perform expensive operations in the window update loop. However, this will also
                // mean you will have to re-register any event handlers on the window elements when re-enabled in OnEnable.
                // gameObject.SetActive(value);

                // Update the Flight AppBar button state
                GameObject.Find(AlarmClockForKSP2Plugin.ToolbarFlightButtonID)
                    ?.GetComponent<UIValue_WriteBool_Toggle>()
                    ?.SetValue(value);

                // Update the OAB AppBar button state
                GameObject.Find(AlarmClockForKSP2Plugin.ToolbarOabButtonID)
                    ?.GetComponent<UIValue_WriteBool_Toggle>()
                    ?.SetValue(value);
            }
        }

        /// <summary>
        /// Runs when the window is first created, and every time the window is re-enabled.
        /// </summary>
        private void OnEnable()
        {
            // Get the UIDocument component from the game object
            _window = GetComponent<UIDocument>();

            // Get the root element of the window.
            _rootElement = _window.rootVisualElement[0];
            _alarmsWindow = _rootElement.Q<VisualElement>("alarms-window");
            
            _alarmsListController = new AlarmsListController(this);
            _newAlarmMenuController = new NewAlarmMenuController(this);
            _transferWindowMenuController = new TransferWindowMenuController(this);
            _customAlarmMenuController = new CustomAlarmMenuController(this);

            _alarmsWindow.Add(_alarmsListController);
            _alarmsWindow.Add(_newAlarmMenuController);
            _alarmsWindow.Add(_transferWindowMenuController);
            _alarmsWindow.Add(_customAlarmMenuController);

            AlarmsList = _alarmsListController.AlarmsListView;

            if (_alarmsWindow != null)
            {
                _alarmsWindow.style.display = DisplayStyle.None;
            }
            else
            {
                AlarmClockForKSP2Plugin.Instance.SWLogger.LogError("Alarms Window was null");
            }

            Button closeButton = _rootElement.Q<Button>("close-button");
            if (closeButton != null)
            {
                closeButton.clicked += () => this.IsWindowOpen = false;
            }

            // Center the window by default
            _rootElement.CenterByDefault();

            _rootElement.style.display = DisplayStyle.None;

            IsWindowOpen = false;
        }

        public void RefreshVisibility(int state)
        {
            _windowState = state;

            switch (state)
            {
                case 0:
                    _alarmsListController.IsVisible = true;
                    _newAlarmMenuController.IsVisible = false;
                    _transferWindowMenuController.IsVisible = false;
                    _customAlarmMenuController.IsVisible = false;
                    break;
                case 1:
                    _alarmsListController.IsVisible = false;
                    _newAlarmMenuController.IsVisible = true;
                    _transferWindowMenuController.IsVisible = false;
                    _customAlarmMenuController.IsVisible = false;
                    break;
                case 2:
                    _alarmsListController.IsVisible = false;
                    _newAlarmMenuController.IsVisible = false;
                    _transferWindowMenuController.IsVisible = true;
                    _customAlarmMenuController.IsVisible = false;
                    break;
                case 3:
                    _alarmsListController.IsVisible = false;
                    _newAlarmMenuController.IsVisible = false;
                    _transferWindowMenuController.IsVisible = false;
                    _customAlarmMenuController.IsVisible = true;
                    break;
            }
        }
    }
}
