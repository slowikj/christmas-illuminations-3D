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
    public class ObjectChangedEventArgs : EventArgs
    {
        public readonly Vector3 Position;
        public readonly Vector3 LookAt;

        public ObjectChangedEventArgs(Vector3 position, Vector3 lookAt)
        {
            this.Position = position;
            this.LookAt = lookAt;
        }
    }
}
