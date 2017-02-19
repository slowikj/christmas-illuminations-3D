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
    public class DefaultDrawer : Drawer
    {
        public DefaultDrawer(DrawingKit drawingKit, Effect effect)
            : base(drawingKit, effect)
        {
        }

        public override void Draw(SceneActor drawableObject)
        {
            _SetEffectParameters(drawableObject);

            MeshesInfo meshInfo = drawableObject.CurrentMesh;

            for(int i = 0; i < meshInfo.LocalToGlobalMatrices.Count; ++i)
            {
                _SetWorldMatrices(meshInfo.LocalToGlobalMatrices[i], drawableObject.WorldMatrix);

                _DrawTriangles(meshInfo.SmoothVertices[i]);
            }
        }
    }
}
