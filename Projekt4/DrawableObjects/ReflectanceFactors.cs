using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projekt4.DrawableObjects
{
    public class ReflectanceFactors
    {
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public float Shininess { get; set; }

        public ReflectanceFactors(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        {
            this.Ambient = ambient;
            this.Diffuse = diffuse;
            this.Specular = specular;
            this.Shininess = shininess;
        }

        public ReflectanceFactors()
            : this(new Vector3((float)0.01, (float)0.01, (float)0.01),
                   new Vector3((float)0.9, (float)0.9, (float)0.9),
                   new Vector3((float)0.1, (float)0.1, (float)0.1),
                   200)
        {
        }
    }
}
