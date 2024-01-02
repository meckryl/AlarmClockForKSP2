using UnityEngine.UIElements;
using UnityEngine;
using UitkForKsp2.API;

namespace AlarmClockForKSP2
{

    public class AlertController : MonoBehaviour
    {
        private UIDocument _window;

        private VisualElement _rootElement;

        private Label _alertLabel;

        private Button _openAlarmsButton;
        private Button _closeButton;

        private bool _isWindowOpen = false;

        public bool IsWindowOpen
        {
            get => _isWindowOpen;
            set
            {
                _isWindowOpen = value;

                _rootElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        private void OnEnable()
        {
            _window = GetComponent<UIDocument>();

            _rootElement = _window.rootVisualElement[0];

            _alertLabel = _rootElement.Q<Label>("alert-label");

            _openAlarmsButton = _rootElement.Q<Button>("to-alarms-button");
            _closeButton = _rootElement.Q<Button>("close-button");

            _openAlarmsButton.clicked += OpenAlarmsClicked;
            _closeButton.clicked += CloseButtonClicked;

            _rootElement.CenterByDefault();

            _rootElement.style.display = DisplayStyle.None;

            IsWindowOpen = false;
        }

        private void OpenAlarmsClicked()
        {
            AlarmClockForKSP2Plugin.Instance.OpenMainWindow();
            IsWindowOpen = false;
        }

        private void CloseButtonClicked()
        {
            IsWindowOpen = false;
        }

        public void DisplayAlert(string title)
        {
            _alertLabel.text = title;
            IsWindowOpen = true;
        }
    }
}
