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

        NormalShockAndCompressiblityRelations shockAndCompressibility;

        public RocketNozzleNormalShockSim()
        {
            GetDataEntry();

            if (ErrorConditionsInDataEntered())
                return;

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

            machAreaRelation = new MachAreaRelation(gamma, exitAreaRatio, 0.1);
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
    }
}
