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
        public LightingInfo Lighting { get; private set; }

        public Illumination(Model model, IEnumerable<Vector3> positions, Color color, ReflectanceFactors reflectanceFactors)
        {
            _lamps = _GetLamps(model, positions, color, reflectanceFactors);
            this.Lighting = _GetLightingInfo(positions, color);
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

        private LightingInfo _GetLightingInfo(IEnumerable<Vector3> positions, Color color)
        {
            return new LightingInfo(positions.ToArray(), positions.Select(p => color.ToVector3()).ToArray());
        }
    }
}
