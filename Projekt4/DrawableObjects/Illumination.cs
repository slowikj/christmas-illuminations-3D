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

namespace Projekt4.DrawableObjects
{
    public class Illumination : IDrawable
    {
        private List<SceneActor> _lamps;
        public IEnumerable<Vector3> LightPositions
        {
            get { return _lamps.Select(lamp => lamp.Position); }
        }

        public IEnumerable<Vector3> LightColors
        {
            get { return _lamps.Select(lamp => lamp.Color.ToVector3()); }
        }

        public Illumination(Model model, IEnumerable<Vector3> positions, Color color, ReflectanceFactors reflectanceFactors = null)
        {
            _lamps = _GetLamps(model, positions, color,
                reflectanceFactors ?? new ReflectanceFactors(new Vector3((float)0.01, (float)0.01, (float)0.01),
                                                             new Vector3((float)0.5, (float)0.5, (float)0.5),
                                                             new Vector3((float)0.5, (float)0.5, (float)0.5),
                                                             2));
        }
        
        public void Draw(Drawer drawer)
        {
            foreach (SceneActor lamp in _lamps)
            {
                lamp.Draw(drawer);
            }
        }

        private List<SceneActor> _GetLamps(Model model, IEnumerable<Vector3> positions, Color color, ReflectanceFactors reflectanceFactors)
        {

            return positions.Select(position =>
                new SceneActor(new Model[] { model }, position, color, reflectanceFactors))
                            .ToList();
        }
        
    }
}
