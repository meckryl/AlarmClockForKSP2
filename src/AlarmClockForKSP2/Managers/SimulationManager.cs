using KSP.Game;
using KSP.Messages;
using KSP.Sim.impl;
using KSP.Sim.Maneuver;

namespace AlarmClockForKSP2.Managers
{
    public static class SimulationManager
    {
        public static VesselComponent ActiveVessel;
        public static ManeuverNodeData CurrentManeuver;

        public static double SOIChangePrediction;
        public static bool SOIChangePredictionExists;

        public static void UpdateActiveVessel()
        {
            ActiveVessel = GameManager.Instance?.Game?.ViewController?.GetActiveVehicle(true)?.GetSimVessel(true);
        }

        public static void UpdateCurrentManeuver(MessageCenterMessage obj)
        {
            UpdateActiveVessel();
            CurrentManeuver = ActiveVessel != null ? GameManager.Instance?.Game?.SpaceSimulation.Maneuvers.GetNodesForVessel(ActiveVessel.GlobalId).FirstOrDefault() : null;
        }

        public static void UpdateSOIChangePrediction(MessageCenterMessage obj)
        {
            UpdateSOIChangePrediction();
        }

        public static void UpdateSOIChangePrediction()
        {
            SOIChangePrediction = ActiveVessel.Orbit.UniversalTimeAtSoiEncounter;
            SOIChangePredictionExists = SOIChangePrediction >= 0;
        }
    }
}
