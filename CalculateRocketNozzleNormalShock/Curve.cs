using System;
using System.Collections.Generic;
using System.Linq;

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
