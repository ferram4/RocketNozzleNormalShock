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
    class Program
    {
        static void Main()
        {
            System.Console.WriteLine("Welcome to Rocket Nozzle Normal Shock Location Calculator");
            System.Console.WriteLine("");
            bool cont = true;
            while (cont)
            {
                if (SingleOrVariableSim())
                {
                    VariableSweepSim varSim = new VariableSweepSim();
                }
                else
                {
                    SingleConditionSim singleSim = new SingleConditionSim();
                }

                cont = Continue();
            }
        }

        //Returns true form VariableSim
        static bool SingleOrVariableSim()
        {
            string input = "";
            do
            {
                System.Console.WriteLine("Select Single Condition Sim (\"SCS\") or Variable Condition Sim (\"VCS\"):");
                input = System.Console.ReadLine();
                input.ToLowerInvariant();
                System.Console.WriteLine("");
            } while (!(input == "scs" || input == "vcs"));

            return input == "vcs";
        }

        static bool Continue()
        {
            char ch;
            do
            {
                System.Console.Write("Continue? (Y/N) ");
                int input = System.Console.ReadLine()[0];
                ch = Convert.ToChar(input);
                System.Console.WriteLine("");
            } while (ch != 'y' && ch != 'Y' && ch != 'n' && ch != 'N');

            return (ch == 'y' || ch == 'Y');
        }
    }
}
