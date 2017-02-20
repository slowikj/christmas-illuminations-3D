using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Projekt4.Cameras;
using Projekt4.DrawableObjects;
using Projekt4.Drawers;

namespace Projekt4.DrawableObjects
{
    public class Scene : IDrawable
    {
        const int LIGHT_PERCENTAGE = 20;

        private List<IDrawable> _drawableObjects;
        private LightingInfo _lightingInfo;

        private SceneActor _currentObject;
        private CameraManager _cameraManager;

        public SceneActor CurrentObject
        {
            get { return _currentObject; }
        }

        public Scene()
        {
            _drawableObjects = new List<IDrawable>();
            _cameraManager = new CameraManager();
            _lightingInfo = new LightingInfo();
        }

        public void AddCamera(String name, Camera camera)
        {
            _cameraManager.AddCamera(name, camera);
        }

        public void SetCamera(String name)
        {
            _cameraManager.SetCamera(name);
        }

        public void AddIllumination(Illumination illumination)
        {
            _drawableObjects.Add(illumination);

            _AddPartOfLightsFrom(illumination.LightPositions.ToArray(),
                illumination.LightColors.ToArray());
        }

        public void AddLight(Vector3 position, Vector3 color)
        {
            _lightingInfo.AddLight(position, color);
        }

        public void AddObject(Model[] models, Vector3 position, Color color,
            ReflectanceFactors reflectanceFactors = null, RotationInfo rotationInfo = null)
        {
            this.AddObject(new SceneActor(models, position, color, reflectanceFactors, rotationInfo));
        }

        public void AddObject(SceneActor drawableObject)
        {
            _drawableObjects.Add(drawableObject);

            if (_currentObject == null)
            {
                _currentObject = drawableObject;
            }
        }

        public void AddObject(IDrawable drawable)
        {
            _drawableObjects.Add(drawable);
        }

        public void Draw(Drawer drawer)
        {
            drawer.SetView(_cameraManager.ViewerPosition, _cameraManager.ViewMatrix);
            drawer.SetLightingInfo(_lightingInfo);

            foreach(IDrawable drawableObject in _drawableObjects)
            {
                drawableObject.Draw(drawer);
            }
        }

        public void MoveCurrentObject(Move move)
        {
            switch (move)
            {
                case Move.Backward: _currentObject.MoveBackward(); break;
                case Move.Forward: _currentObject.MoveForward(); break;
                case Move.RotateClockwise: _currentObject.RotateClockwise(); break;
                case Move.RotateAnticlockwise: _currentObject.RotateAntiClockwise(); break;
            }
        }
        
        private void _AddPartOfLightsFrom(Vector3[] positions, Vector3[] colors)
        {
            int inc = positions.Length / (LIGHT_PERCENTAGE * positions.Length / 100);
            for(int i = 0; i < positions.Length; i += inc)
            {
                this.AddLight(positions[i], colors[i]);
            }
        }

    }
}
