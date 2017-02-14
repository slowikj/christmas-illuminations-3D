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

namespace Projekt4
{
    public class DrawableObject
    {
        public MeshesInfo MeshesInfo { get; private set; }
        public Matrix WorldMatrix { get; private set; }
        public Matrix WorldInverseTranspose { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 ViewVector { get; private set; }

        public RotationInfo RotationInfo { get; private set; }

        private const float _INC_ANGLE = (float)0.05;
        private readonly Vector3 _DEFAULT_FORWARD_VECTOR = new Vector3(0, 0, -1);

        public ReflectanceFactors ReflectanceFactors { get; set; }

        public Color Color { get; private set; }
        
        public DrawableObject(Model model, Vector3 position, Color color, ReflectanceFactors reflectanceFactors,
            RotationInfo rotationInfo)
        {
            this.MeshesInfo = new MeshesInfo(model, color);

            this.Color = color;
            this.RotationInfo = rotationInfo;
            this.ReflectanceFactors = reflectanceFactors;
            this.Position = position;
            
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

            _UpdateObject();
        }

        public void MoveBackward()
        {
            this.Position -= this.ViewVector;

            _UpdateObject();
        }
        
        private void _UpdateObject()
        {
            this.WorldMatrix = _GetWorldMatrix(this.Position,
                this.RotationInfo.XRotate,
                this.RotationInfo.YRotate,
                this.RotationInfo.ZRotate);

            this.WorldInverseTranspose = _GetTransposedInversedMatrix(this.WorldMatrix);

            float angle = this.RotationInfo.YRotate;

            this.ViewVector = new Vector3((float)(-_DEFAULT_FORWARD_VECTOR.X * Math.Cos(angle) + _DEFAULT_FORWARD_VECTOR.Z * Math.Sin(angle)),
                _DEFAULT_FORWARD_VECTOR.Y,
                (float)(-_DEFAULT_FORWARD_VECTOR.Z * Math.Sin(angle) + _DEFAULT_FORWARD_VECTOR.Z * Math.Cos(angle)));

            this.ViewVector.Normalize();
            this.ViewVector /= 8;
         
        }

        private Matrix _GetWorldMatrix(Vector3 position,
            float xRotate, float yRotate, float zRotate)
        {
            return Matrix.CreateRotationX(xRotate)
                * Matrix.CreateRotationY(yRotate)
                * Matrix.CreateRotationZ(zRotate)
                * Matrix.CreateTranslation(position);
        }

        private Matrix _GetTransposedInversedMatrix(Matrix matrix)
        {
            return Matrix.Transpose(Matrix.Invert(matrix));
        }
    }
}
