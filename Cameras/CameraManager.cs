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
    public class CameraManager
    {
        private Dictionary<String, Camera> _cameras;
        private Camera _currentCamera;

        public Matrix ViewMatrix
        {
            get { return _currentCamera.ViewMatrix; }
        }

        public Vector3 ViewerPosition
        {
            get { return _currentCamera.CameraPosition; }
        }

        public CameraManager()
        {
            _cameras = new Dictionary<string, Camera>();
            _currentCamera = null;
        }

        public void AddCamera(String name, Camera camera)
        {
            _cameras.Add(name, camera);
        }

        public void SetCamera(String name)
        {
            _currentCamera = _cameras[name];
        }
    }
}
