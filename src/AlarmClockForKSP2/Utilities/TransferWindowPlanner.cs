using KSP.Game;
using KSP.Sim.impl;

namespace AlarmClockForKSP2
{

    public static class TransferWindowPlanner
    {

        private struct Range
        {
            public double lower, upper;

            public Range(double lwr, double upr)
            {
                lower = lwr;
                upper = upr;
            }
        }

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

            double nextWindow = LambertSolver.NextLaunchWindowUT(originBody, destinationBody);

            return nextWindow;
        }
    }
}
