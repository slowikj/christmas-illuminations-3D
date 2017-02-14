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

        public DrawingKit(GraphicsDevice graphicsDevice, LightingInfo lightingInfo)
            : this(_GetDefaultViewMatrix(), _GetDefaultProjectionMatrix(graphicsDevice), _GetDefaultViewerPosition(),
                  graphicsDevice, lightingInfo)
        {
        }

        private static Matrix _GetDefaultViewMatrix()
        {
            return Matrix.CreateLookAt(
                new Vector3(0, 0, 1),
                Vector3.Zero,
                Vector3.UnitY);
        }

        private static Vector3 _GetDefaultViewerPosition()
        {
            return new Vector3(0, 0, 1);
        }

        private static Matrix _GetDefaultProjectionMatrix(GraphicsDevice graphicsDevice)
        {
            return Matrix.CreatePerspectiveFieldOfView(
                 MathHelper.PiOver4,
                 graphicsDevice.Viewport.AspectRatio,
                 0.1f,
                 200f);
        }
    }
}
