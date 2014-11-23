using System;
using System.Collections.Generic;

namespace CalculateRocketNozzleNormalShock
{
    class MachAreaRelation
    {
        Dictionary<int, double> areaFunctionOfMach;
        Dictionary<double, int> machFunctionOfArea;

        double MstepSize;

        public MachAreaRelation(double gamma, double exitAreaRatio, double MstepSize)
        {
            this.MstepSize = MstepSize;
            
            double exponent = (gamma + 1) / (gamma - 1);
            double machFactor = (gamma - 1) * 0.5;
            double compressibleFactor = 2 / (gamma + 1);

            double mach = 1;
            double areaRatio = 1;
            int stepsAboveM1 = 0;

            areaFunctionOfMach = new Dictionary<int, double>();
            machFunctionOfArea = new Dictionary<double, int>();

            areaFunctionOfMach.Add(0, 1);
            machFunctionOfArea.Add(1, 0);

            while(areaRatio < exitAreaRatio)        //calculate Mach-Area relationship and add to dictionaries
            {
                mach += MstepSize;
                stepsAboveM1++;

                areaRatio = mach * mach;
                areaRatio *= machFactor;
                areaRatio++;

                areaRatio *= compressibleFactor;
                areaRatio = Math.Pow(areaRatio, exponent);
                areaRatio /= (mach * mach);

                areaRatio = Math.Sqrt(areaRatio);

                areaFunctionOfMach.Add(stepsAboveM1, areaRatio);
                machFunctionOfArea.Add(areaRatio, stepsAboveM1);
            }

            System.Console.WriteLine("Mach-Area Relationship initialized:");
            System.Console.WriteLine("AreaRatio Bounds: " + 1 + " to " + areaRatio);
            System.Console.WriteLine("Mach Number Bounds: " + 1 + " to " + mach);
        }
    }
}
