using AlarmClockForKSP2.Managers;
using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class NewAlarmMenuController : VisualElement
    {
        private WindowController _parentController;

        private bool _isVisible = false;

        public Button ManeuverButton;
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
                ManeuverButton = this.Q<Button>("maneuver-button");
                ManeuverButton.clicked += ManeuverButtonClicked;

                SoiButton = this.Q<Button>("soi-button");
                SoiButton.clicked += SOIButtonClicked;

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

        private void ManeuverButtonClicked()
        {
            if (SimulationManager.CurrentManeuver == null)
            {
                _parentController.RefreshVisibility(0);
                return;
            }

            double maneuverTimeSeconds = SimulationManager.CurrentManeuver.Time;
            TimeManager.Instance.AddAlarm($"{SimulationManager.ActiveVessel.Name} reaches maneuver", maneuverTimeSeconds);
            _parentController.RefreshVisibility(0);
        }

        private void SOIButtonClicked()
        {
            SimulationManager.UpdateSOIChangePrediction();
            if (!SimulationManager.SOIChangePredictionExists)
            {
                _parentController.RefreshVisibility(0);
                return;
            }

            double soiChangeTimeSeconds = SimulationManager.SOIChangePrediction;
            TimeManager.Instance.AddAlarm($"{SimulationManager.ActiveVessel.Name} changes SOI", soiChangeTimeSeconds);
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
