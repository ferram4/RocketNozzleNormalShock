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
using System.Collections.Generic;

namespace CalculateRocketNozzleNormalShock
{
    class RocketNozzleNormalShockSim : NozzleSim
    {
        public RocketNozzleNormalShockSim(double exitAreaRatio, double gamma) : base(exitAreaRatio, gamma) { }

        public bool TryFindNormalShock(out double areaRatio, double chamberPressure, double backPressure)
        {
            this.chamberPressure = chamberPressure;
            this.backPressure = backPressure;

            areaRatio = 0;
            if (ErrorConditionsInDataEntered())
                return false;

            areaRatio = IterateToSolution();
            return true;
        }
    
        private bool ErrorConditionsInDataEntered()
        {
            if(backPressure > chamberPressure)
            {
                System.Console.WriteLine("Error: back pressure was higher than chamber pressure; ending sim");
                System.Console.WriteLine("");
                return true;
            }
            if(backPressure < 0)
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

            if(exitPWithNormShock >= backPressure)
            {
                System.Console.WriteLine("Back pressure too low to create shock in nozzle; ending sim");
                System.Console.WriteLine("");
                return true;
            }
            return false;
        }


        private double IterateToSolution()
        {
            double testRatio, lowerRatio, upperRatio;
            lowerRatio = 1;
            upperRatio = exitAreaRatio;
            testRatio = (upperRatio + lowerRatio) * 0.5;

            int counter = 0;

            int result;
            do
            {
                System.Console.Write("Iter: " + counter + " AreaRatio: " + testRatio);
                counter++;
                result = TestAreaRatio(testRatio, PRES_TOLERANCE);
                if (result > 0)
                {
                    lowerRatio = testRatio;
                }
                else if (result < 0)
                {
                    upperRatio = testRatio;
                }

                testRatio = (upperRatio + lowerRatio) * 0.5;

            } while (result != 0 && counter <= MAX_ITERATIONS);

            System.Console.WriteLine("");
            System.Console.WriteLine("Sim completed; iterations: " + counter);
            System.Console.WriteLine("Area Ratio w/ shock: " + testRatio);
            System.Console.WriteLine("");

            return testRatio;
        }

        private int TestAreaRatio(double areaRatio, double pressureTolerance)
        {
            //calculate conditions upsteam of the shock if it were to exist at this location
            double upstreamShockM = machAreaRelation.EvaluateMachNumberSupersonic(areaRatio);
            double upstreamShockP = chamberPressure / shockAndCompressibility.StagnationPressureRatio(upstreamShockM);

            //Get conditions downstream of the shock
            double downstreamShockM = shockAndCompressibility.MachNumberDownstreamOfShock(upstreamShockM);
            double downstreamShockP = shockAndCompressibility.PressureRatioAcrossShock(upstreamShockM) * upstreamShockP;

            double downstreamShockEffectiveAreaRatio = machAreaRelation.EvaluateAreaRatio(downstreamShockM);        //Calculate the areaRatio that would create this Mach number;
            double effectiveActualThroatRatio = areaRatio / downstreamShockEffectiveAreaRatio;                  //This is a ratio between the throat of the true nozzle, and the effective nozzle this is now looking at

            double effectiveExitRatio = exitAreaRatio / effectiveActualThroatRatio;
            double exitM = machAreaRelation.EvaluateMachNumberSubsonic(effectiveExitRatio);         //using the effective exit area ratio, get the Mach number at the exit

            double pressureRatioDownstreamShockToExit = shockAndCompressibility.StagnationPressureRatio(downstreamShockM) / shockAndCompressibility.StagnationPressureRatio(exitM);
            double pressureAtExit = downstreamShockP * pressureRatioDownstreamShockToExit;

            double diffExitPresBackPres = pressureAtExit - backPressure;
            System.Console.WriteLine(" Pressure Diff: " + diffExitPresBackPres);

            if (Math.Abs(diffExitPresBackPres) < pressureTolerance)
                return 0;
            else if (diffExitPresBackPres < 0)
            {
                return -1;
            }
            else
                return 1;
        }
    }
}
