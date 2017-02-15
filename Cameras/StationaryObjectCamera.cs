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
    public class StationaryObjectCamera : Camera
    {
        public StationaryObjectCamera(Vector3 position, DrawableObject connectedObject)
            : base(position, connectedObject.Position)
        {
            connectedObject.ObjectChanged += this.ObjectChangedHandler;
        }
        public override void ObjectChangedHandler(object sender, ObjectChangedEventArgs eventArgs)
        {
            _SetCamera(this.CameraPosition, eventArgs.LookAt);
        }
    }
}
