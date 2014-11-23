/*
Copyright (c) 2014, Michael Ferrara
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;

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