using AlarmClockForKSP2.Managers;
using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class NewAlarmContext : ContextElement
    {

        public Button ManeuverButton;
        public Button SoiButton;
        public Button TransferWindowButton;
        public Button CustomAlarmButton;

        public NewAlarmContext(Action<int> swapContext) : base(swapContext, "alarmclock-resources/UI/NewAlarmMenu.uxml")
        {
            ManeuverButton = this.Q<Button>("maneuver-button");
            ManeuverButton.clicked += ManeuverButtonClicked;

            SoiButton = this.Q<Button>("soi-button");
            SoiButton.clicked += SOIButtonClicked;

            TransferWindowButton = this.Q<Button>("transfer-window-button");
            TransferWindowButton.clicked += TransferWindowButtonClicked;

            CustomAlarmButton = this.Q<Button>("custom-button");
            CustomAlarmButton.clicked += CustomAlarmButtonClicked;
        }
        private void DefaultToListView()
        {
            _swapContext((int)MainWindowContext.AlarmsList);
        }

        private void ManeuverButtonClicked()
        {
            if (SimulationManager.CurrentManeuver == null)
            {
                _swapContext((int)MainWindowContext.AlarmsList);
                return;
            }

            double maneuverTimeSeconds = SimulationManager.CurrentManeuver.Time;
            TimeManager.Instance.AddAlarm($"{SimulationManager.ActiveVessel.Name} reaches maneuver", maneuverTimeSeconds);
            _swapContext((int)MainWindowContext.AlarmsList);
        }

        private void SOIButtonClicked()
        {
            SimulationManager.UpdateSOIChangePrediction();
            if (!SimulationManager.SOIChangePredictionExists)
            {
                _swapContext((int)MainWindowContext.AlarmsList);
                return;
            }

            double soiChangeTimeSeconds = SimulationManager.SOIChangePrediction;
            TimeManager.Instance.AddAlarm($"{SimulationManager.ActiveVessel.Name} changes SOI", soiChangeTimeSeconds);
            _swapContext((int)MainWindowContext.AlarmsList);
        }

        private void TransferWindowButtonClicked()
        {
            _swapContext((int)MainWindowContext.TransferWindow);
        }

        private void CustomAlarmButtonClicked()
        {
            _swapContext((int)MainWindowContext.Custom);
        }
    }
}
