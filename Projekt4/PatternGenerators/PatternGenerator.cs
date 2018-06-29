using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projekt4.PatternGenerators
{
    public class PatternGenerator
    {
        private Function2D _getX, _getY, _getZ;

        public PatternGenerator(Function2D getX, Function2D getY, Function2D getZ)
        {
            _getX = getX;
            _getY = getY;
            _getZ = getZ;
        }

        public IEnumerable<Vector3> GetPoints(int numberOfPoints)
        {
            return _getZ.GetResults(numberOfPoints)
                .Zip(_getY.GetResults(numberOfPoints).Zip(_getX.GetResults(numberOfPoints), (y, x) => new Vector2(x, y)),
                     (z, v) => new Vector3(v.X, v.Y, z));
        }

    }
}
