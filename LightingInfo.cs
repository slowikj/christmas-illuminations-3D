using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Projekt4
{
    public class LightingInfo
    {
        public Vector3[] Positions { get; private set; }
        public Vector3[] Colors { get; private set; }

        public int Count { get { return Positions.Length; } }

        public LightingInfo(Vector3[] positions, Vector3[] colors)
        {
            this.Positions = positions;
            this.Colors = colors;
        }
    }
}
