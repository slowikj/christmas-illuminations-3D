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

namespace Projekt4.Drawers
{
    public class SmoothDrawer : Drawer
    {
        public SmoothDrawer(DrawingKit drawingKit, Effect effect)
            : base(drawingKit, effect)
        {
        }

        public override void Draw(DrawableObject drawableObject)
        {
            _SetEffectParameters(drawableObject);
            
            for(int i = 0; i < drawableObject.MeshesInfo.LocalToGlobalMatrices.Count; ++i)
            {
                Matrix resultWorld = drawableObject.MeshesInfo.LocalToGlobalMatrices[i] * drawableObject.WorldMatrix;
                _effect.Parameters["World"].SetValue(resultWorld);
                _effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(resultWorld)));

                _DrawTriangles(drawableObject.MeshesInfo.SmoothTriangles[i]);
            }
        }
    }
}
