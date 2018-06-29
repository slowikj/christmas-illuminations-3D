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
    public class DrawingKit
    {
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Vector3 ViewerPosition { get; set; }

        public GraphicsDevice GraphicsDevice { get; set; }

        public LightingInfo LightingInfo { get; set; }
        public DrawingKit(Matrix viewMatrix, Matrix projectionMatrix,
            Vector3 viewerPosition, GraphicsDevice graphicsDevice, LightingInfo lightingInfo)
        {
            this.ViewMatrix = viewMatrix;
            this.ProjectionMatrix = projectionMatrix;
            this.ViewerPosition = viewerPosition;
            this.GraphicsDevice = graphicsDevice;
            this.LightingInfo = lightingInfo;
        }
        

        public static Matrix GetDefaultProjectionMatrix(GraphicsDevice graphicsDevice)
        {
            return Matrix.CreatePerspectiveFieldOfView(
                 MathHelper.PiOver4,
                 graphicsDevice.Viewport.AspectRatio,
                 0.1f,
                 200f);
        }
    }
}
