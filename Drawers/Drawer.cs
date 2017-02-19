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
    public abstract class Drawer
    {
        private DrawingKit _drawingKit;
        protected Effect _effect;

        public Drawer(DrawingKit drawingKit, Effect effect)
        {
            _drawingKit = drawingKit;
            _effect = effect;
        }

        public void SetView (Vector3 viewerPosition, Matrix viewMatrix)
        {
            _drawingKit.ViewerPosition = viewerPosition;
            _drawingKit.ViewMatrix = viewMatrix;
        }

        public void SetLightingInfo(LightingInfo lightingInfo)
        {
            _drawingKit.LightingInfo = lightingInfo;
        }

        public void SetPhongIllumination()
        {
            _effect.Parameters["PhongIllumination"].SetValue(true);
        }

        public void SetBlinnIllumination()
        {
            _effect.Parameters["PhongIllumination"].SetValue(false);
        }

        public abstract void Draw(SceneActor drawableObject);

        protected void _DrawTriangles(VertexPositionNormalColor[] triangleList)
        {
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                _drawingKit.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList, 0, triangleList.Length / 3);
            }
        }
        
        protected void _SetEffectParameters(SceneActor drawableObject)
        {
            _effect.Parameters["View"].SetValue(_drawingKit.ViewMatrix);
            _effect.Parameters["ViewerPosition"].SetValue(_drawingKit.ViewerPosition);
            _effect.Parameters["Projection"].SetValue(_drawingKit.ProjectionMatrix);
            _effect.Parameters["LightsNum"].SetValue(_drawingKit.LightingInfo.Count);
            _effect.Parameters["LightPosition"].SetValue(_drawingKit.LightingInfo.Positions);
            _effect.Parameters["LightColor"].SetValue(_drawingKit.LightingInfo.Colors);
            _effect.Parameters["Shininess"].SetValue(drawableObject.ReflectanceFactors.Shininess);
            _effect.Parameters["ka"].SetValue(drawableObject.ReflectanceFactors.Ambient);
            _effect.Parameters["kd"].SetValue(drawableObject.ReflectanceFactors.Diffuse);
            _effect.Parameters["ks"].SetValue(drawableObject.ReflectanceFactors.Shininess);
        }

        protected void _SetWorldMatrices(Matrix localToGlobalMatrix, Matrix objectWorldMatrix)
        {
            Matrix resultWorld = localToGlobalMatrix * objectWorldMatrix;
            _effect.Parameters["World"].SetValue(resultWorld);
            _effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(resultWorld)));
        }
    }
}
