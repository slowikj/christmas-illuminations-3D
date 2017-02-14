using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4
{
    public class RotationInfo
    {

        public float XRotate { get; set; }
        public float YRotate { get; set; }
        public float ZRotate { get; set; }

        public RotationInfo(float xRotate, float yRotate, float zRotate)
        {
            this.XRotate = xRotate;
            this.YRotate = yRotate;
            this.ZRotate = zRotate;
        }
    }
}
