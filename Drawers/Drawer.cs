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
    public abstract class Drawer
    {
        private DrawingKit _drawingKit;
        protected Effect _effect;

        public Drawer(DrawingKit drawingKit, Effect effect)
        {
            _drawingKit = drawingKit;
            _effect = effect;
        }

        public abstract void Draw(DrawableObject drawableObject);

        protected void _Draw(VertexPositionNormalColor[] triangleList)
        {
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                _drawingKit.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList, 0, triangleList.Length / 3);
            }
        }

        protected void _SetEffectParameters(DrawableObject drawableObject)
        {
            _effect.Parameters["World"].SetValue(drawableObject.WorldMatrix);
            _effect.Parameters["View"].SetValue(_drawingKit.ViewMatrix);
            _effect.Parameters["ViewerPosition"].SetValue(_drawingKit.ViewerPosition);
            _effect.Parameters["Projection"].SetValue(_drawingKit.ProjectionMatrix);
            _effect.Parameters["WorldInverseTranspose"].SetValue(drawableObject.WorldInverseTranspose);
            _effect.Parameters["LightsNum"].SetValue(_drawingKit.LightingInfo.Count);
            _effect.Parameters["LightPosition"].SetValue(_drawingKit.LightingInfo.Positions);
            _effect.Parameters["LightColor"].SetValue(_drawingKit.LightingInfo.Colors);
            _effect.Parameters["Shininess"].SetValue(drawableObject.ReflectanceFactors.Shininess);
            _effect.Parameters["ka"].SetValue(drawableObject.ReflectanceFactors.Ambient);
            _effect.Parameters["kd"].SetValue(drawableObject.ReflectanceFactors.Diffuse);
            _effect.Parameters["ks"].SetValue(drawableObject.ReflectanceFactors.Shininess);
        }
    }
}
