using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt4.DrawableObjects;

namespace Projekt4.Cameras
{
    public class MoveableObjectCamera : Camera
    {
        public MoveableObjectCamera(SceneActor drawableObject)
            : base(drawableObject.Position, drawableObject.Position + drawableObject.ViewVector)
        {
            drawableObject.ObjectChanged += this.ObjectChangedHandler;
        }
        public override void ObjectChangedHandler(object sender, ObjectChangedEventArgs eventArgs)
        {
            _SetCamera(eventArgs.Position, eventArgs.LookAt);
        }
    }
}
