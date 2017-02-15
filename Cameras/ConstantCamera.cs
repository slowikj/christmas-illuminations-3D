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
    public class ConstantCamera : Camera
    {
        public ConstantCamera(Vector3 position, Vector3 cameraTarget)
            : base(position, cameraTarget)
        {
        }

        public ConstantCamera()
            : base()
        {
        }

        public override void ObjectChangedHandler(object sender, ObjectChangedEventArgs eventArgs)
        {
        }
    }
}
