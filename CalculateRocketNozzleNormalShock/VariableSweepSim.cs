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
    class VariableSweepSim
    {
        SweepSimDataEntry data;
        RocketNozzleNormalShockSim shockSim;
        EmpiricalShockAndFlowSepSim empiricalSim;

        public VariableSweepSim()
        {
            DataEntry entry = new DataEntry();
            data = entry.GetDataSweepSimEntry();

            bool useEmpirical = ShockOrEmpiricalSim();
            if (useEmpirical)
            {
                empiricalSim = new EmpiricalShockAndFlowSepSim(data.exitAreaRatio, data.gamma);
                empiricalSim.SetCurrentModel();
            }
            else
            {
                shockSim = new RocketNozzleNormalShockSim(data.exitAreaRatio, data.gamma);
            }

            string[,] dataArray = IterateConditions(useEmpirical);

            System.Console.WriteLine("Choose FileName for Data, including extension: ");
            string fileName = Console.ReadLine();
            System.Console.WriteLine("");
            DataWriting write = new DataWriting();

            write.WriteToFile(fileName, dataArray);
        }

        private string[,] IterateConditions(bool useEmpirical)
        {
            double chamberIncPerStep = (data.chamberPressureMax - data.chamberPressureMin) / (double)data.chamberPressureSteps;
            double backIncPerStep = (data.backPressureMax - data.backPressureMin) / (double)data.backPressureSteps;

            string[,] dataToWrite = new string[data.chamberPressureSteps + 2, data.backPressureSteps + 2];

            dataToWrite[0, 0] = "";
            double soln;
            for(int i = 0; i <= data.backPressureSteps; i++)
            {
                double backPressure = data.backPressureMin + i * backIncPerStep;
                dataToWrite[0, i + 1] = backPressure.ToString();

                for(int j = 0; j <= data.chamberPressureSteps; j++)
                {

                    double chamberPressure = data.chamberPressureMin + j * chamberIncPerStep;
                    dataToWrite[j + 1, 0] = chamberPressure.ToString();
                    if (useEmpirical)
                    {
                        if (empiricalSim.TryCalculateEmpiricalModel(out soln, chamberPressure, backPressure))
                        {
                            dataToWrite[j + 1, i + 1] = soln.ToString();
                        }
                        else
                            dataToWrite[j + 1, i + 1] = "NULL";

                    }
                    else
                    {
                        if (shockSim.TryFindNormalShock(out soln, chamberPressure, backPressure))
                        {
                            dataToWrite[j + 1, i + 1] = soln.ToString();
                        }
                        else
                            dataToWrite[j + 1, i + 1] = "NULL";
                    }
                }
            }

            return dataToWrite;
        }

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
