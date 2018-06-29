using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4.Drawers
{
    public class DrawerManager
    {
        private Dictionary<String, Drawer> _drawers;
        public Drawer CurrentDrawer { get; private set; }

        public DrawerManager()
        {
            _drawers = new Dictionary<string, Drawer>();
        }

        public void AddDrawer(String name, Drawer drawer)
        {
            _drawers.Add(name, drawer);
        }

        public void SetDrawer(String name)
        {
            this.CurrentDrawer = _drawers[name];
        }

        public void SetPhongIllumination()
        {
            foreach(Drawer drawer in _drawers.Values)
            {
                drawer.SetPhongIllumination();
            }
        }

        public void SetBlinnIllumination()
        {
            foreach(Drawer drawer in _drawers.Values)
            {
                drawer.SetBlinnIllumination();
            }

        }
    }
}
