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
    class NozzleSim
    {
        protected MachAreaRelation machAreaRelation;
        protected NormalShockAndCompressiblityRelations shockAndCompressibility;
        protected double exitAreaRatio;
        protected double gamma;

        protected double chamberPressure;
        protected double backPressure;

        protected const double MACH_INCREMENT = 0.001;
        protected const double PRES_TOLERANCE = 0.001;    //In kPa
        protected const int MAX_ITERATIONS = 1000;

        public NozzleSim(double exitAreaRatio, double gamma)
        {
            this.exitAreaRatio = exitAreaRatio;
            this.gamma = gamma;

            machAreaRelation = new MachAreaRelation(gamma, exitAreaRatio, MACH_INCREMENT);
            shockAndCompressibility = new NormalShockAndCompressiblityRelations(gamma);
        }

        protected bool ErrorConditionsInDataEntered()
        {
            if (backPressure > chamberPressure)
            {
                System.Console.WriteLine("Error: back pressure was higher than chamber pressure; ending sim");
                System.Console.WriteLine("");
                return true;
            }
            if (backPressure < 0)
            {
                System.Console.WriteLine("Error: back pressure was below vacuum; ending sim");
                System.Console.WriteLine("");
                return true;
            }

            return CheckPressureAtExit();
        }

        private bool CheckPressureAtExit()
        {
            double exitM = machAreaRelation.EvaluateMachNumberSupersonic(exitAreaRatio);

            double exitP = shockAndCompressibility.StagnationPressureRatio(exitM);      //chamber pressure is stagnation pressure
            exitP = chamberPressure / exitP;                                            //use the stagnation pressure relationship to calculate pressure at exit

            if (exitP > backPressure)
            {
                System.Console.WriteLine("Exit pressure was greater than ambient; no shock in nozzle, ending sim");
                System.Console.WriteLine("");
                return true;
            }
            double exitPWithNormShock = shockAndCompressibility.PressureRatioAcrossShock(exitM);
            exitPWithNormShock = exitP * exitPWithNormShock;

            if (exitPWithNormShock >= backPressure)
            {
                System.Console.WriteLine("Back pressure too low to create shock in nozzle; ending sim");
                System.Console.WriteLine("");
                return true;
            }
            return false;
        }
    }
}
