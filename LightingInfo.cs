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
        private List<Vector3> _positions, _colors;
        public Vector3[] Positions
        {
            get { return _positions.ToArray(); }
        }

        public Vector3[] Colors
        {
            get { return _colors.ToArray(); }
        }

        public int Count
        {
            get { return _positions.Count; }
        }

        public LightingInfo()
        {
            _positions = new List<Vector3>();
            _colors = new List<Vector3>();
        }

        public void AddLight(Vector3 position, Vector3 color)
        {
            _positions.Add(position);
            _colors.Add(color);
        }
    }
}
