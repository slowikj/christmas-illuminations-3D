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

namespace Projekt4.Cameras
{
    public class Camera
    {
        protected readonly Vector3 _DEFAULT_CAMERA_POSITION = new Vector3(0, 0, 3);
        protected readonly Vector3 _DEFAULT_CAMERA_TARGET = new Vector3(0, 0, 0); 
        public Matrix ViewMatrix { get; protected set; }
        public Vector3 CameraPosition { get; protected set; }
        
        public Camera()
        {
            _SetCamera(_DEFAULT_CAMERA_POSITION, _DEFAULT_CAMERA_TARGET);
        }
        
        public Camera(Vector3 position, Vector3 cameraTarget)
        {
            _SetCamera(position, cameraTarget);
        }
        
        protected void _SetCamera(Vector3 cameraPosition, Vector3 cameraTarget)
        {
            this.CameraPosition = cameraPosition;
            this.ViewMatrix = _GetViewMatrix(cameraPosition, cameraTarget);
        }
        protected Matrix _GetViewMatrix(Vector3 cameraPosition, Vector3 cameraTarget)
        {
            return Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.UnitY);
        }
    }
}
