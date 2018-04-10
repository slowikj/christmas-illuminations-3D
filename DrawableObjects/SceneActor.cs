using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Projekt4.Drawers;
using Projekt4.Cameras;

namespace Projekt4.DrawableObjects
{
    public class SceneActor : IDrawable
    {
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;
        private MeshesInfo[] _meshesInfo;
        private int _currentFrame;
        public MeshesInfo CurrentMesh
        {
            get { return _meshesInfo[_currentFrame]; }
        }
        public Matrix WorldMatrix { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 ViewVector { get; private set; }

        public RotationInfo RotationInfo { get; private set; }

        private const float _INC_ANGLE = (float)0.05;
        private readonly Vector3 _DEFAULT_FORWARD_VECTOR = new Vector3(0, 0, -1);

        public ReflectanceFactors ReflectanceFactors { get; set; }

        public Color Color { get; private set; }
        
        public SceneActor(Model[] models, Vector3 position, Color color, ReflectanceFactors reflectanceFactors = null,
            RotationInfo rotationInfo = null)
        {
            _meshesInfo = models.Select(model => new MeshesInfo(model, color)).ToArray();
            _SetParameters(position, color, reflectanceFactors, rotationInfo);
        }

        private void _SetParameters(Vector3 position, Color color, ReflectanceFactors reflectanceFactors, RotationInfo rotationInfo)
        {
            this.Color = color;
            this.RotationInfo = rotationInfo ?? new RotationInfo(0, 0, 0);
            this.ReflectanceFactors = reflectanceFactors ?? new ReflectanceFactors();

            this.Position = position;

            _currentFrame = 0;

            _UpdateObject();
        }
        
        public void Draw(Drawer drawer)
        {
            drawer.Draw(this);
        }

        public void RotateClockwise()
        {
            this.Rotate(-_INC_ANGLE);
        }

        public void RotateAntiClockwise()
        {
            this.Rotate(_INC_ANGLE);
        }

        public void Rotate(float angle = _INC_ANGLE)
        {
            this.RotationInfo.YRotate += angle;

            _UpdateObject();
        }

        public void MoveForward()
        {
            this.Position += this.ViewVector;

            _currentFrame = (_currentFrame + 1 == this._meshesInfo.Length
                ? 0 : _currentFrame + 1);

            _UpdateObject();
        }

        public void MoveBackward()
        {
            this.Position -= this.ViewVector;

            _currentFrame = (_currentFrame == 0
                ? _currentFrame = this._meshesInfo.Length - 1 : _currentFrame - 1);

            _UpdateObject();
        }

        public float GetWidth()
        {
            return this.CurrentMesh.MaxX - this.CurrentMesh.MinX;
        }

        public float GetLength()
        {
            return this.CurrentMesh.MaxZ - this.CurrentMesh.MinZ;
        }
        
        private void _UpdateObject()
        {
            this.WorldMatrix = _GetWorldMatrix(this.Position,
                this.RotationInfo.XRotate,
                this.RotationInfo.YRotate,
                this.RotationInfo.ZRotate);

            float angle = this.RotationInfo.YRotate;

            this.ViewVector = new Vector3((float)(-_DEFAULT_FORWARD_VECTOR.X * Math.Cos(angle) + _DEFAULT_FORWARD_VECTOR.Z * Math.Sin(angle)),
                _DEFAULT_FORWARD_VECTOR.Y,
                (float)(-_DEFAULT_FORWARD_VECTOR.Z * Math.Sin(angle) + _DEFAULT_FORWARD_VECTOR.Z * Math.Cos(angle)));

            this.ViewVector.Normalize();
            this.ViewVector /= 8;

            this.FireObjectChangedEvent();         
        }

        private void FireObjectChangedEvent()
        {
            this.ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(this.Position, this.Position + this.ViewVector));
        }

        private Matrix _GetWorldMatrix(Vector3 position,
            float xRotate, float yRotate, float zRotate)
        {
            return Matrix.CreateRotationX(xRotate)
                * Matrix.CreateRotationY(yRotate)
                * Matrix.CreateRotationZ(zRotate)
                * Matrix.CreateTranslation(position);
        }
    }
}
