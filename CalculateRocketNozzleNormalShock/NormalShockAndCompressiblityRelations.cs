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
