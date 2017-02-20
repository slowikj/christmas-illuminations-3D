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
    public struct VertexPositionNormalColor : IVertexType
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Color Color { get; set; }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexPositionNormalColor.VertexDeclaration;
            }
        }

        public VertexPositionNormalColor(Vector3 position, Vector3 normal, Color color)
        {
            this.Position = position;
            this.Normal = normal;
            this.Color = color;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(sizeof(float) * 6, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        );
    }
}
