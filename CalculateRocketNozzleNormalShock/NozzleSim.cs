using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateRocketNozzleNormalShock
{
    class NozzleSim
    {
        protected MachAreaRelation machAreaRelation;
        protected NormalShockAndCompressiblityRelations shockAndCompressibility;
        protected double exitAreaRatio;
        protected double gamma;

        protected double chamberPressure;
        protected double backPressure;

        protected const double MACH_INCREMENT = 0.001;
        protected const double PRES_TOLERANCE = 0.001;    //In kPa
        protected const int MAX_ITERATIONS = 1000;

        public NozzleSim(double exitAreaRatio, double gamma)
        {
            this.exitAreaRatio = exitAreaRatio;
            this.gamma = gamma;

            machAreaRelation = new MachAreaRelation(gamma, exitAreaRatio, MACH_INCREMENT);
            shockAndCompressibility = new NormalShockAndCompressiblityRelations(gamma);
        }

        protected bool ErrorConditionsInDataEntered()
        {
            if (backPressure > chamberPressure)
            {
                System.Console.WriteLine("Error: back pressure was higher than chamber pressure; ending sim");
                System.Console.WriteLine("");
                return true;
            }
            if (backPressure < 0)
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

            if (exitPWithNormShock >= backPressure)
            {
                System.Console.WriteLine("Back pressure too low to create shock in nozzle; ending sim");
                System.Console.WriteLine("");
                return true;
            }
            return false;
        }
    }
}
