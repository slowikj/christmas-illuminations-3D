using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Projekt4.Drawers
{
    public class FlatDrawer : Drawer
    {
        public FlatDrawer(DrawingKit drawingKit, Effect effect)
            : base(drawingKit, effect)
        {
        }

        public override void Draw(DrawableObject drawableObject)
        {
            _SetEffectParameters(drawableObject);
            _Draw(drawableObject.FlatTriangles);
        }
    }
}
