using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Projekt4.DrawableObjects
{
    public class MeshesInfo
    {
        public List<VertexPositionNormalColor[]> SmoothTriangles { get; private set; }
        public List<VertexPositionNormalColor[]> FlatTriangles { get; private set; }
        public List<Vector3[]> SidePositions { get; private set; }
        public List<Matrix> LocalToGlobalMatrices { get; private set; }


        public MeshesInfo(Model model, Color color)
        {
            this.SmoothTriangles = _GetSmoothTriangles(model, color);
            this.FlatTriangles = _GetFlatTriangles(model, color);
            this.SidePositions = _GetSidePositions(this.SmoothTriangles);
            this.LocalToGlobalMatrices = _GetLocalToGlobalMatrices(model);
        }
        
        private List<VertexPositionNormalColor[]> _GetSmoothTriangles(Model model, Color color)
        {
            return _GetTriangles(model,
                (vertices, indices) => _GetDefaultTriangles(color, vertices, indices));
        }
        
        private List<VertexPositionNormalColor[]> _GetFlatTriangles(Model model, Color color)
        {
            Func<VertexPositionNormalColor[], short[], List<VertexPositionNormalColor>> prepareTriangles =
             (vertices, indices) =>
             {
                 List<VertexPositionNormalColor> res = new List<VertexPositionNormalColor>();
                 for(int i = 0; i < indices.Length; i += 3)
                 {
                     Vector3 averageNormal = _GetAverageOfVectors(
                         vertices[indices[i + 0]].Normal,
                         vertices[indices[i + 1]].Normal,
                         vertices[indices[i + 2]].Normal);
                     
                     for(int j = 0; j < 3; ++j)
                     {
                         res.Add(new VertexPositionNormalColor(vertices[indices[i + j]].Position, averageNormal, color));
                     }
                 }

                 return res;
             };

            return _GetTriangles(model, prepareTriangles);
        }

        private List<Matrix> _GetLocalToGlobalMatrices(Model model)
        {
            return model.Meshes.Select(mesh => mesh.ParentBone.ModelTransform).ToList();
        }
        private List<VertexPositionNormalColor> _GetDefaultTriangles(Color color, VertexPositionNormalColor[] vertices, short[] indices)
        {
            List<VertexPositionNormalColor> res = new List<VertexPositionNormalColor>();
            foreach (short index in indices)
            {
                res.Add(new VertexPositionNormalColor(vertices[index].Position,
                    vertices[index].Normal,
                    color));
            }

            return res;
        }

        private List<VertexPositionNormalColor[]> _GetTriangles(Model model,
            Func<VertexPositionNormalColor[], short[], List<VertexPositionNormalColor>> prepareTriangles)
        {
            List<VertexPositionNormalColor[]> res = new List<VertexPositionNormalColor[]>();

            foreach (ModelMesh mesh in model.Meshes)
            {
                res.Add(mesh.MeshParts.SelectMany(part => _GetVerticesFromPart(part, prepareTriangles)).ToArray());
            }

            return res;
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

        private List<Vector3[]> _GetSidePositions(List<VertexPositionNormalColor[]> verticesParts)
        {
            List<Vector3[]> res = new List<Vector3[]>();
            
            foreach(var part in verticesParts)
            {
                res.Add(_GetSidePositionsFromPart(part));
            }

            return res;
        }

        private Vector3[] _GetSidePositionsFromPart(VertexPositionNormalColor[] vertices)
        {
            List<Vector3> res = new List<Vector3>();
            for(int i = 0; i < vertices.Length; i += 3)
            {
                res.Add(_GetAverageOfVectors(vertices[i].Position, vertices[i + 1].Position, vertices[i + 2].Position));
            }

            return res.ToArray();
        }
        
        private Vector3 _GetAverageOfVectors(params Vector3[] vectors)
        {
            return vectors.Aggregate((a, b) => a + b) / vectors.Length;
        }
    }
}
