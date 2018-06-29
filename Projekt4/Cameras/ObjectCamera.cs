using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Projekt4.DrawableObjects;

namespace Projekt4.Cameras
{
    public abstract class ObjectCamera : Camera
    {
        public ObjectCamera(SceneActor drawableObject)
            : base(drawableObject.Position, drawableObject.Position + drawableObject.ViewVector)
        {
            drawableObject.ObjectChanged += this.ObjectChangedHandler;
        }

        protected ObjectCamera(Vector3 position, Vector3 cameraTarget)
            : base(position, cameraTarget)
        {
        }

        public abstract void ObjectChangedHandler(Object sender, ObjectChangedEventArgs eventArgs);
    }
}
