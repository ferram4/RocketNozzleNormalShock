using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateRocketNozzleNormalShock
{
    class RocketNozzleNormalShockSim
    {
        MachAreaRelation machAreaRelation;
        double exitAreaRatio;
        double gamma;

        double chamberPressure;
        double backPressure;

        const double MACH_INCREMENT = 0.001;
        const double PRES_TOLERANCE = 0.001;    //In kPa

        NormalShockAndCompressiblityRelations shockAndCompressibility;

        public RocketNozzleNormalShockSim()
        {
            GetDataEntry();

            if (ErrorConditionsInDataEntered())
                return;

            IterateToSolution();
        }

        private void GetDataEntry()
        {

            System.Console.WriteLine("Beginning new nozzle sim...");
            System.Console.WriteLine("");

            System.Console.WriteLine("Please enter the following information:");
            System.Console.WriteLine("");
            exitAreaRatio = GetDoubleData("Nozzle Area Ratio: ");
            gamma = GetDoubleData("Exhaust Ratio of Specific Heats (gamma): ");
            System.Console.WriteLine("");
            chamberPressure = GetDoubleData("Engine Chamber Pressure (kPa): ") * 1000;
            backPressure = GetDoubleData("Ambient Pressure (kPa): ") * 1000;
            System.Console.WriteLine("");

            machAreaRelation = new MachAreaRelation(gamma, exitAreaRatio, MACH_INCREMENT);
            shockAndCompressibility = new NormalShockAndCompressiblityRelations(gamma);
            System.Console.WriteLine("");

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
            if(PressureAtExitAboveBackPressure())
            {
                System.Console.WriteLine("Back pressure was greater than ambient; no shock in nozzle");
                System.Console.WriteLine("");
                return true;

            }
            return false;
        }

        private bool PressureAtExitAboveBackPressure()
        {
            double exitM = machAreaRelation.EvaluateMachNumberSupersonic(exitAreaRatio);

            double exitP = shockAndCompressibility.StagnationPressureRatio(exitM);      //chamber pressure is stagnation pressure
            exitP = chamberPressure / exitP;                                            //use the stagnation pressure relationship to calculate pressure at exit

            return exitP > backPressure;
        }

        private void IterateToSolution()
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

            } while (result != 0);
            System.Console.WriteLine("");
            System.Console.WriteLine("Sim completed; iterations: " + counter);
            System.Console.WriteLine("Area Ratio w/ shock: " + testRatio);
            System.Console.WriteLine("");
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
