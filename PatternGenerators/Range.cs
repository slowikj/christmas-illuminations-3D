using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4.PatternGenerators
{
    public struct Range
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public Range(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}
