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
        public VertexPositionNormalColor[] SmoothTriangles { get; private set; }
        public VertexPositionNormalColor[] FlatTriangles { get; private set; }
        public Matrix WorldMatrix { get; private set; }
        public Matrix WorldInverseTranspose { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 ViewVector { get; private set; }

        public RotationInfo RotationInfo { get; private set; }

        private const float _INC_ANGLE = (float)0.05;
        private readonly Vector3 _DEFAULT_FORWARD_VECTOR = new Vector3(0, 0, -1);

        public ReflectanceFactors ReflectanceFactors { get; set; }

        private Color _color;

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                for(int i = 0; i < this.SmoothTriangles.Length; ++i)
                {
                    this.SmoothTriangles[i].Color = this.FlatTriangles[i].Color = value;
                }
            }
        }
        
        public DrawableObject(Model model, Vector3 position, Color color, ReflectanceFactors reflectanceFactors,
            RotationInfo rotationInfo)
        {
            _PrepareTriangleLists(model, color);
            _color = color;
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
  
        private void _PrepareTriangleLists(Model model, Color color)
        {
            this.SmoothTriangles = _GetTriangles(model, color,
                (vertices, indices) =>
                {
                    List<VertexPositionNormalColor> res = new List<VertexPositionNormalColor>();
                    foreach (short index in indices)
                    {
                        res.Add(new VertexPositionNormalColor(vertices[index].Position,
                            vertices[index].Normal,
                            color));
                    }

                    return res;
                });

            this.FlatTriangles = _GetTriangles(model, color,
                (vertices, indices) =>
                {
                    List<VertexPositionNormalColor> res = new List<VertexPositionNormalColor>();
                    foreach (short index in indices)
                    {
                        res.Add(new VertexPositionNormalColor(vertices[index].Position,
                            (res.Count % 3 == 0 ? vertices[index].Normal : res[res.Count - 1].Normal),
                            color));
                    }

                    return res;
                });
        }
        
        private VertexPositionNormalColor[] _GetTriangles(Model model, Color color,
            Func<VertexPositionNormalColor[], short[], List<VertexPositionNormalColor>> prepareTriangles)
        {
            List<VertexPositionNormalColor> res = new List<VertexPositionNormalColor>(); 
            
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    res.AddRange(_GetVerticesFromPart(part, prepareTriangles));
                }
            }

            return res.ToArray();
        }

        private List<VertexPositionNormalColor> _GetVerticesFromPart(ModelMeshPart part,
            Func<VertexPositionNormalColor[], short[], List<VertexPositionNormalColor>> prepareTriangles)
        {
            VertexDeclaration declaration = part.VertexBuffer.VertexDeclaration;
            VertexElement[] vertexElements = declaration.GetVertexElements();
            VertexElement vertexPosition = new VertexElement();

            VertexPositionNormalColor[] vertices = new VertexPositionNormalColor[part.NumVertices];

            part.VertexBuffer.GetData<VertexPositionNormalColor>(
                part.VertexOffset * declaration.VertexStride + vertexPosition.Offset,
                vertices,
                0,
                part.NumVertices,
                declaration.VertexStride);

            short[] indices = new short[part.PrimitiveCount * 3];
            part.IndexBuffer.GetData<short>(
                part.StartIndex * 2,
                indices,
                0,
                part.PrimitiveCount * 3);

            return prepareTriangles(vertices, indices);
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
