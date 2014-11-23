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
        //machAreaRelation = new MachAreaRelation(1.4, 40, 0.1);

        public RocketNozzleNormalShockSim()
        {
            GetDataEntry();

            if (NoNormalShock())
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

            machAreaRelation = new MachAreaRelation(1.4, 40, 0.1);
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

        private bool NoNormalShock()
        {
            
        }
    }
}
