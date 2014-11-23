using System;
using System.Collections.Generic;
using System.Text;


namespace CalculateRocketNozzleNormalShock
{
    class Program
    {
        static RocketNozzleNormalShockSim currentSim;
        static void Main()
        {
            System.Console.WriteLine("Welcome to Rocket Nozzle Normal Shock Location Calculator");
            System.Console.WriteLine("");
            bool cont = true;
            while (cont)
            {
                currentSim = new RocketNozzleNormalShockSim();

                cont = Continue();
            }
        }

        static bool Continue()
        {
            char ch;
            do
            {
                System.Console.Write("Continue? (Y/N)");
                int input = System.Console.Read();
                ch = Convert.ToChar(input);
                System.Console.WriteLine("");
            } while (ch != 'y' && ch != 'Y' && ch != 'n' && ch != 'N');

            return (ch == 'y' || ch == 'Y');
        }
    }
}
