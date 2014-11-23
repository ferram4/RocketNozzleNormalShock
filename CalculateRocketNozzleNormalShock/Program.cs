using System;
using System.Collections.Generic;
using System.Text;


namespace CalculateRocketNozzleNormalShock
{
    class Program
    {
        static MachAreaRelation machAreaRelation;
        static void Main()
        {
            System.Console.WriteLine("Prepare For Normal Shock Calculations...");
            machAreaRelation = new MachAreaRelation(1.4, 40, 0.1);
        }
    }
}
