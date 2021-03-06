﻿/*
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
    class SingleConditionSim
    {
        SingleSimDataEntry data;
        public SingleConditionSim()
        {
            DataEntry entry = new DataEntry();
            data = entry.GetDataSingleSimEntry();

            if (ShockOrEmpiricalSim())
            {
                EmpiricalShockAndFlowSepSim sim = new EmpiricalShockAndFlowSepSim(data.exitAreaRatio, data.gamma);
                double shockSoln;
                sim.SetCurrentModel();
                sim.TryCalculateEmpiricalModel(out shockSoln, data.chamberPressure, data.backPressure);

            }
            else
            {
                RocketNozzleNormalShockSim sim = new RocketNozzleNormalShockSim(data.exitAreaRatio, data.gamma);
                double shockSoln;
                sim.TryFindNormalShock(out shockSoln, data.chamberPressure, data.backPressure);
            }
        }

        //Returns true for Empirical
        private bool ShockOrEmpiricalSim()
        {
            string input = "";
            do
            {
                System.Console.WriteLine("Select \"Shock\" or \"Empirical\" simulation:");
                input = System.Console.ReadLine();
                input.ToLowerInvariant();
                System.Console.WriteLine("");
            } while (!(input == "shock" || input == "empirical"));

            return input == "empirical";
        }
    }
}
