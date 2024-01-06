using KSP.UI.Binding;
using UitkForKsp2.API;
using UnityEngine;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public enum MainWindowContext
    {
        AlarmsList,
        NewAlarm,
        TransferWindow,
        Custom,
        Timer,
        Settings
    }
    public class WindowController : MonoBehaviour
    {
        // The UIDocument component of the window game object
        private UIDocument _window;

        // The elements of the window that we need to access
        private VisualElement _rootElement;

        private VisualElement _alarmsWindow;

        public AlarmsListContext AlarmsListController;
        private NewAlarmContext _newAlarmMenuController;
        private TransferWindowContext _transferWindowMenuController;
        private TimerAlarmContext _customAlarmMenuController;
        private TimerAlarmContext _timerAlarmContext;
        private SettingsMenuContext _settingsMenuController;

        private List<ContextElement> _contexts = new List<ContextElement>();

        public ListView AlarmsList;
        public MainWindowContext PreviousState;
        public MainWindowContext CurrentState;

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
                SwapContext((int)MainWindowContext.AlarmsList);

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

            _settingsMenuController = new SettingsMenuContext(SwapContext);


            AlarmsListController = new AlarmsListContext(SwapContext);
            _newAlarmMenuController = new NewAlarmContext(SwapContext);
            _transferWindowMenuController = new TransferWindowContext(SwapContext, _settingsMenuController.GetSettings);
            _customAlarmMenuController = new TimerAlarmContext(SwapContext);
            _timerAlarmContext = new TimerAlarmContext(SwapContext);

            _contexts.Add(AlarmsListController);
            _contexts.Add(_newAlarmMenuController);
            _contexts.Add(_transferWindowMenuController);
            _contexts.Add(_customAlarmMenuController);
            _contexts.Add(_timerAlarmContext);
            _contexts.Add(_settingsMenuController);

            foreach (ContextElement context in _contexts) _alarmsWindow.Add(context);

            AlarmsList = AlarmsListController.AlarmsListView;

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

        private void SwapContext(int state)
        {
            PreviousState = CurrentState;
            CurrentState = (MainWindowContext)state;
            for(int index = 0; index<_contexts.Count; index++)
            {
                _contexts[index].IsVisible = index == state;
            }
        }
    }
}
