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
    }
}
