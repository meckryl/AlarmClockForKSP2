using AlarmClockForKSP2.Managers;
using KSP.Game;
using KSP.Sim.impl;

namespace AlarmClockForKSP2
{
    public static class TransferWindowPlanner
    {
        public const float GRAVITATIONAL_CONSTANT = 6.67e-11f;

        private static int orbitalTreeDepth(CelestialBodyComponent body)
        {
            if (body.GetRelevantStar() == null) return 0;
            if (body.Orbit.referenceBody.Name == body.GetRelevantStar().Name) return 1;
            return 2; //For now assume that moons don't have other bodies in orbit around them
        }

        private static CelestialBodyComponent findNearestParent(CelestialBodyComponent originBody, CelestialBodyComponent destinationBody)
        {
            int originDepth = orbitalTreeDepth(originBody);
            int destinationDepth = orbitalTreeDepth(destinationBody);

            if (originDepth == 0) return originBody;
            if (destinationDepth == 0) return destinationBody;

            CelestialBodyComponent originBodyDummy = originBody;
            CelestialBodyComponent destinationBodyDummy = destinationBody;

            for (int nodesTraveresed = 0; nodesTraveresed < originDepth - destinationDepth; nodesTraveresed++)
                originBodyDummy = originBodyDummy.Orbit.referenceBody;

            for (int nodesTraveresed = 0; nodesTraveresed < destinationDepth - originDepth; nodesTraveresed++)
                destinationBodyDummy = destinationBodyDummy.Orbit.referenceBody;

            while (originBodyDummy.Name != destinationBodyDummy.Name)
            {
                originBodyDummy = originBodyDummy.Orbit.referenceBody;
                destinationBodyDummy = destinationBodyDummy.Orbit.referenceBody;
            }

            return originBodyDummy;
        }

        /// <summary>
        /// Original code by:
        /// https://github.com/ABritInSpace/TransferCalculator-KSP2
        /// License: https://raw.githubusercontent.com/ABritInSpace/TransferCalculator-KSP2/master/LICENSE.md
        /// </summary>
        private static double idealTransferAngle(CelestialBodyComponent originBody, CelestialBodyComponent destinationBody, CelestialBodyComponent referenceBody)
        {
            double meanSemiMajorAxis = (originBody.Orbit.semiMajorAxis + destinationBody.Orbit.semiMajorAxis) / 2;
            double time = MathF.PI * MathF.Sqrt((float)(meanSemiMajorAxis * meanSemiMajorAxis * meanSemiMajorAxis) / ((float)referenceBody.Mass * GRAVITATIONAL_CONSTANT));

            double transferAngle = (((180 - ((time / destinationBody.Orbit.period) * 360)) % 360) + 360) % 360;

            return transferAngle;
        }

        public static double getNextTransferWindow(string origin, string destination, double currentTime)
        {
            GameInstance game = GameManager.Instance?.Game;
            if (game == null) return -1;

            CelestialBodyComponent originBody = game.UniverseModel.FindCelestialBodyByName(origin);
            CelestialBodyComponent destinationBody = game.UniverseModel.FindCelestialBodyByName(destination);
            if (originBody == null || destinationBody == null) return -1;

            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Origin: {originBody.Name}");
            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Destination: {destinationBody.Name}");

            //If origin body and reference body are equal, or one body is in orbit around the other, then all times are euqally viable for transfer
            if (originBody.Name == destinationBody.Name) return 0;
            if (originBody.Name == destinationBody.Orbit.referenceBody.Name || originBody.Orbit.referenceBody.Name == destinationBody.Name) return 0;

            //If the origin and target are at different stars then default to -1 for now
            if (originBody.GetRelevantStar().Name != destinationBody.GetRelevantStar().Name) return -1;

            CelestialBodyComponent referenceBody = findNearestParent(originBody, destinationBody);
            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Reference: {referenceBody.Name}");

            if (originBody.Orbit.referenceBody.Name != referenceBody.Name) originBody = originBody.Orbit.referenceBody;
            if (destinationBody.Orbit.referenceBody.Name != referenceBody.Name) destinationBody = destinationBody.Orbit.referenceBody;

            double transferAngle = idealTransferAngle(originBody, destinationBody, referenceBody);

            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"Transfer Angle: {transferAngle}");

            //Temporarily convert to hours to avoid rounding issues
            double originDegreesPerHour = 360 * 3600 / originBody.Orbit.period;
            double destinationDegreesPerHour = 360 * 3600 / destinationBody.Orbit.period;

            double relativeAngularSpeed = destinationDegreesPerHour - originDegreesPerHour;

            double currentPhase = Vector3d.SignedAngle((Vector3d)destinationBody.Position.localPosition, (Vector3d)originBody.Position.localPosition, Vector3d.up);
            double targetPhase = (360 + transferAngle) % 360;

            double nextWindow;

            if (relativeAngularSpeed > 0)
            {
                if (targetPhase > currentPhase)
                {
                    nextWindow = currentTime + 3600 * (targetPhase - currentPhase) / (relativeAngularSpeed);
                }
                else
                {
                    nextWindow = currentTime + 3600 * (360 + targetPhase - currentPhase) / (relativeAngularSpeed);
                }
            }
            else
            {
                if (targetPhase > currentPhase)
                {
                    nextWindow = currentTime + 3600 * (targetPhase - currentPhase - 360) / (relativeAngularSpeed);
                }
                else
                {
                    nextWindow = currentTime + 3600 * (targetPhase - currentPhase) / (relativeAngularSpeed);
                }
            }

            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"{relativeAngularSpeed} - {currentPhase} - {targetPhase} - {nextWindow}");

            return nextWindow;
        }
    }
}
