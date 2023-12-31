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

        public static void UpdateActiveVessel(MessageCenterMessage obj)
        {
            ActiveVessel = GameManager.Instance?.Game?.ViewController?.GetActiveVehicle(true)?.GetSimVessel(true);
        }

        public static void UpdateCurrentManeuver(MessageCenterMessage obj)
        {
            UpdateActiveVessel(obj);
            CurrentManeuver = ActiveVessel != null ? GameManager.Instance?.Game?.SpaceSimulation.Maneuvers.GetNodesForVessel(ActiveVessel.GlobalId).FirstOrDefault() : null;
        }
    }
}
