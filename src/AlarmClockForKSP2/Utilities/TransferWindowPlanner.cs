using System;
using System.Collections.Generic;
using System.Text;

namespace AlarmClockForKSP2
{
   enum Planet
    {
        Moho,
        Eve,
        Kerbin,
        Duna,
        Dres,
        Jool,
        Eeloo
    }
    public struct orbitalBody
    {
        public orbitalBody(int index, double[] targets, double lengthOfOrbit, double initialPhase, string name)
        {
            Index = index;
            Targets = targets;
            LengthOfOrbit = lengthOfOrbit;
            InitialPhase = initialPhase;
            Name = name;
        }

        public int Index;
        public double[] Targets;
        public double LengthOfOrbit;
        public double InitialPhase;
        public string Name;
    }
    public static class TransferWindowPlanner
    {
        public static orbitalBody[] planets;

        public static void instantiateBodies()
        {
            planets = new orbitalBody[7];

            double[] mohoTargets = { double.NaN, 58.49, 76.05, 90.64, 103.67, 108.92, 110.7 };
            planets[0] = new orbitalBody(
                (int)Planet.Moho,
                mohoTargets,
                615.49,
                84.92,
                "Moho");

            double[] eveTargets = { 230.87, double.NaN, 36.07, 66.07, 92.04, 102.24, 105.67 };
            planets[1] = new orbitalBody(
                (int)Planet.Eve,
                eveTargets,
                1571.7,
                15.00,
                "Eve");

            double[] kerbinTargets = { 108.21, 305.87, double.NaN, 44.36, 82.06, 96.58, 101.42 };
            planets[2] = new orbitalBody(
                (int)Planet.Kerbin,
                kerbinTargets,
                2556.50,
                0,
                "Kerbin");

            double[] dunaTargets = { -158.32, -168.68, -75.19, double.NaN, 62.21, 85.52, 93.19 };
            planets[3] = new orbitalBody(
                (int)Planet.Duna,
                dunaTargets,
                4809.80,
                135.51,
                "Duna");

            double[] dresTargets = { -29.86, -204.51, -329.68, -145.8, double.NaN, 51.95, 68.52 };
            planets[4] = new orbitalBody(
                (int)Planet.Dres,
                dresTargets,
                13303.60,
                10.02, 
                "Dres");

            double[] joolTargets = { -297.62, -178.48, -48.65, -31.06, -99.83, double.NaN, 31.01 };
            planets[5] = new orbitalBody(
                (int)Planet.Jool,
                joolTargets,
                29072.60,
                238.43, 
                "Jool");

            double[] eelooTargets = { -49.76, -82.55, -80.33, -247.09, -185.43, -43.49, double.NaN };
            planets[6] = new orbitalBody(
                (int)Planet.Eeloo,
                eelooTargets,
                43608.90,
                309.98,
                "Eeloo");
        }

        public static double getNextTransferWindow(orbitalBody origin, orbitalBody destination, double currentTime)
        {
            if (planets is null) instantiateBodies();

            AlarmClockForKSP2Plugin.Instance.SWLogger.LogMessage($"{origin.Name} : {origin.Index}, {destination.Name} : {destination.Index}");
            if (origin.Index == destination.Index) return -1;

            double originDegreesPerHour = 360 / origin.LengthOfOrbit;
            double destinationDegreesPerHour = 360 / destination.LengthOfOrbit;

            double relativeAngularSpeed = destinationDegreesPerHour - originDegreesPerHour;

            double currentPhase = (360 + (destination.InitialPhase - origin.InitialPhase) + (currentTime/3600) * relativeAngularSpeed) % 360;
            double targetPhase = (360 + origin.Targets[destination.Index]) % 360;

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
