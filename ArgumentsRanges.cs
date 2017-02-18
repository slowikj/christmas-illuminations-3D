using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4
{
    public class ArgumentsRanges
    {
        public float XMin { get; private set; }
        public float XMax { get; private set; }
        public float YMin { get; private set; }
        public float YMax { get; private set; }
        public float ZMin { get; private set; }
        public float ZMax { get; private set; }


        public ArgumentsRanges(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;

            this.YMin = yMin;
            this.YMax = yMax;

            this.ZMin = zMin;
            this.ZMax = zMax;
        }
    }
}
