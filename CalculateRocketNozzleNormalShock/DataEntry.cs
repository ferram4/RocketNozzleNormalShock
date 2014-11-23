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
    class DataEntry
    {
        public SingleSimDataEntry GetDataSingleSimEntry()
        {
            SingleSimDataEntry data = new SingleSimDataEntry();
            System.Console.WriteLine("Beginning new single condition nozzle sim...");
            System.Console.WriteLine("");

            System.Console.WriteLine("Please enter the following information:");
            System.Console.WriteLine("");
            data.exitAreaRatio = GetDoubleData("Nozzle Area Ratio: ");
            data.gamma = GetDoubleData("Exhaust Ratio of Specific Heats (gamma): ");

            System.Console.WriteLine("");
            data.chamberPressure = GetDoubleData("Engine Chamber Pressure (kPa): ") * 1000;
            data.backPressure = GetDoubleData("Ambient Pressure (kPa): ") * 1000;
            System.Console.WriteLine("");
            System.Console.WriteLine("");
            return data;
        }

        public SweepSimDataEntry GetDataSweepSimEntry()
        {
            SweepSimDataEntry data = new SweepSimDataEntry();
            System.Console.WriteLine("Beginning new variable sweep nozzle sim...");
            System.Console.WriteLine("");

            System.Console.WriteLine("Please enter the following information:");
            System.Console.WriteLine("");
            data.exitAreaRatio = GetDoubleData("Nozzle Area Ratio: ");
            data.gamma = GetDoubleData("Exhaust Ratio of Specific Heats (gamma): ");

            System.Console.WriteLine("");
            data.chamberPressureMax = GetDoubleData("Engine Max Chamber Pressure (kPa): ") * 1000;
            data.chamberPressureMin = GetDoubleData("Engine Min Chamber Pressure (kPa): ") * 1000;
            data.chamberPressureSteps = Math.Max(GetIntData("Num Chamber Pressure Steps: "), 2);

            System.Console.WriteLine("");
            data.backPressureMax = GetDoubleData("Max Ambient Pressure (kPa): ") * 1000;
            data.backPressureMin = GetDoubleData("Min Ambient Pressure (kPa): ") * 1000;
            data.backPressureSteps = Math.Max(GetIntData("Num Ambient Pressure Steps: "), 2);
            System.Console.WriteLine("");
            System.Console.WriteLine("");
            return data;
        }

        private double GetDoubleData(string dataToRequest)
        {
            string tmp;
            double result;

            bool correctEntry = true;
            do
            {
                System.Console.Write(dataToRequest);
                tmp = System.Console.ReadLine();

                correctEntry = correctEntry = double.TryParse(tmp, out result);
                if (!correctEntry)
                {
                    System.Console.WriteLine("Error parsing; please input a valid number");
                    System.Console.WriteLine("");
                }

            } while (!correctEntry);

            return result;
        }

        private int GetIntData(string dataToRequest)
        {
            string tmp;
            int result;

            bool correctEntry = true;
            do
            {
                System.Console.Write(dataToRequest);
                tmp = System.Console.ReadLine();

                correctEntry = correctEntry = int.TryParse(tmp, out result);
                if (!correctEntry)
                {
                    System.Console.WriteLine("Error parsing; please input a valid integer");
                    System.Console.WriteLine("");
                }

            } while (!correctEntry);

            return result;
        }
    }

    struct SingleSimDataEntry
    {
        public double exitAreaRatio;
        public double gamma;
        public double chamberPressure;
        public double backPressure;
    }

    struct SweepSimDataEntry
    {
        public double exitAreaRatio;
        public double gamma;

        public double chamberPressureMax;
        public double chamberPressureMin;
        public int chamberPressureSteps;

        public double backPressureMax;
        public double backPressureMin;
        public int backPressureSteps;
    }
}
