using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateRocketNozzleNormalShock
{
    class NormalShockAndCompressiblityRelations
    {
        double gamma;

        public NormalShockAndCompressiblityRelations(double gamma)
        {
            this.gamma = gamma;
        }

        public double PressureRatioAcrossShock(double M1)
        {
            double result = M1 * M1;
            result--;
            result *= 2 * gamma;

            result /= (gamma + 1);
            result++;

            return result;
        }

        public double MachNumberDownstreamOfShock(double M1)
        {
            double gammaParam = gamma - 1;
            gammaParam *= 0.5;

            double M2 = M1 * M1;
            M2 *= gamma;
            M2 -= gammaParam;

            M2 = (M1 * M1 * gammaParam + 1) / M2;

            return M2;
        }

        public double StagnationPressureRatio(double M1)
        {
            double result = M1 * M1;
            result *= (gamma - 1);
            result *= 0.5;
            result++;

            result = Math.Pow(result, gamma / (gamma - 1));

            return result;
        }
    }
}
