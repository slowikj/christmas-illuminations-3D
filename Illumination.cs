using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Projekt4.DrawableObjects;
using Projekt4.Drawers;

namespace Projekt4
{
    public class Illumination : IDrawable
    {
        private List<DrawableObject> _lamps;
        public IEnumerable<Vector3> LightPositions
        {
            get { return _lamps.Select(lamp => lamp.Position); }
        }

        public IEnumerable<Vector3> LightColors
        {
            get { return _lamps.Select(lamp => lamp.Color.ToVector3()); }
        }

        public Illumination(Model model, IEnumerable<Vector3> positions, Color color, ReflectanceFactors reflectanceFactors)
        {
            _lamps = _GetLamps(model, positions, color, reflectanceFactors);
        }
        
        public void Draw(Drawer drawer)
        {
            foreach (DrawableObject lamp in _lamps)
            {
                lamp.Draw(drawer);
            }
        }

        private List<DrawableObject> _GetLamps(Model model, IEnumerable<Vector3> positions, Color color, ReflectanceFactors reflectanceFactors)
        {

            return positions.Select(position =>
                new DrawableObject(new Model[] { model }, position, color, reflectanceFactors))
                            .ToList();
        }
        
    }
}
