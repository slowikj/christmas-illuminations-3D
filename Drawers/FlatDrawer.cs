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
    public class FlatDrawer : Drawer
    {
        public FlatDrawer(DrawingKit drawingKit, Effect effect)
            : base(drawingKit, effect)
        {
        }

        public override void Draw(SceneActor drawableObject)
        {
            _SetEffectParameters(drawableObject);

            MeshesInfo meshesInfo = drawableObject.CurrentMesh;
            for(int i = 0; i < meshesInfo.LocalToGlobalMatrices.Count; ++i)
            {
                _SetWorldMatrices(meshesInfo.LocalToGlobalMatrices[i], drawableObject.WorldMatrix);

                for(int j = 0; j < meshesInfo.SidePositions[i].Length; ++j)
                {
                    _effect.Parameters["SidePosition"].SetValue(meshesInfo.SidePositions[i][j]);
                    //_DrawTriangles(meshesInfo.FlatVertices[i].Skip(j * 3).Take(3).ToArray());
                    _DrawTriangles(new VertexPositionNormalColor[] {
                        meshesInfo.FlatVertices[i][j * 3],
                        meshesInfo.FlatVertices[i][j * 3 + 1],
                        meshesInfo.FlatVertices[i][j * 3 + 2]});
                }

            }
        }
    }
}
