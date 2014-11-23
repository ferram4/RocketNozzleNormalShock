using System;
using System.Collections.Generic;

namespace CalculateRocketNozzleNormalShock
{
    class MachAreaRelation
    {
        Curve areaFunctionOfMach;
        Curve machFunctionOfAreaSupersonic;
        Curve machFunctionOfAreaSubsonic;

        double MstepSize;

        public MachAreaRelation(double gamma, double exitAreaRatio, double MstepSize)
        {
            this.MstepSize = MstepSize;
            
            double exponent = (gamma + 1) / (gamma - 1);
            double machFactor = (gamma - 1) * 0.5;
            double compressibleFactor = 2 / (gamma + 1);

            double mach = MstepSize;
            double areaRatio = 1;

            areaFunctionOfMach = new Curve();
            machFunctionOfAreaSupersonic = new Curve();
            machFunctionOfAreaSubsonic = new Curve();

            areaFunctionOfMach.Add(0, 1);
            machFunctionOfAreaSupersonic.Add(1, 0);
            machFunctionOfAreaSubsonic.Add(1, 0);

            while(areaRatio < exitAreaRatio || mach < 1)        //calculate Mach-Area relationship and add to dictionaries
            {
                mach += MstepSize;

                areaRatio = mach * mach;
                areaRatio *= machFactor;
                areaRatio++;

                areaRatio *= compressibleFactor;
                areaRatio = Math.Pow(areaRatio, exponent);
                areaRatio /= (mach * mach);

                areaRatio = Math.Sqrt(areaRatio);

                areaFunctionOfMach.Add(mach, areaRatio);
                if(mach >= 1)
                    machFunctionOfAreaSupersonic.Add(areaRatio, mach);
                if (mach <= 1)
                    machFunctionOfAreaSubsonic.Add(areaRatio, mach);
            }

            System.Console.WriteLine("Mach-Area Relationship initialized:");
            System.Console.WriteLine("AreaRatio Bounds: " + 1 + " to " + areaRatio);
            System.Console.WriteLine("Mach Number Bounds: " + MstepSize + " to " + mach);
        }

        public double EvaluateAreaRatio(double M)
        {
            return areaFunctionOfMach.EvaluateY(M);
        }

        public double EvaluateMachNumberSubsonic(double areaRatio)
        {
            return machFunctionOfAreaSubsonic.EvaluateY(areaRatio);
        }

        public double EvaluateMachNumberSupersonic(double areaRatio)
        {
            return machFunctionOfAreaSupersonic.EvaluateY(areaRatio);
        }
    }
}