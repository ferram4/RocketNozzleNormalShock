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
using System.Collections.Generic;

namespace CalculateRocketNozzleNormalShock
{
    class Curve
    {
        List<xyPair> vals;

        public Curve()
        {
            vals = new List<xyPair>();
        }

        public void Add(double x, double y)
        {
            xyPair pair = new xyPair(x, y);
            vals.Add(pair);
        }

        public void Sort()
        {
            vals.Sort();
        }

        public double EvaluateY(double x)
        {
            int i = 0;
            bool foundSomething = false;
            for(i = 0; i < vals.Count; i++)     //iterate through until reaching an index with an x value greater than this one
            {
                if (vals[i].Item1 < x)
                    continue;
                else
                {
                    foundSomething = true;
                    break;
                }
            }
            if (!foundSomething && i == vals.Count - 1)       //got all the way to the end and found nothing; this use the upper extreme value
                return vals[i].Item2;
            if (i == 0)
                return vals[0].Item2;               //Didn't get anywhere; use the lower extreme value
                
            xyPair upper, lower;
            upper = vals[i];
            lower = vals[i-1];

            double result = (upper.Item2 - lower.Item2) / (upper.Item1 - lower.Item1);
            result *= (x - lower.Item1);
            result += lower.Item2;

            return result;
        }

        class xyPair : Tuple<double, double> , IComparable
        {
            public xyPair(double x, double y) : base(x, y) { }

            public int CompareTo(xyPair other)
            {
                return this.Item1.CompareTo(other.Item1);
            }
        }
    }
}
