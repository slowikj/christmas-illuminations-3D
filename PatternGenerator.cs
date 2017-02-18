using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projekt4
{
    public class PatternGenerator
    {
        private Func<float, float> _getX, _getY, _getZ;
        public PatternGenerator(Func<float, float> getX, Func<float, float> getY, Func<float, float> getZ)
        {
            _getX = getX;
            _getY = getY;
            _getZ = getZ;
        }

        public IEnumerable<Vector3> GetPoints(int pointsNum,
            float xArgMin, float xArgMax,
            float yArgMin, float yArgMax,
            float zArgMin, float zArgMax)
        {
            IEnumerable<float> X = _getResultsOfFunction(_getX, xArgMin, xArgMax, pointsNum);

            IEnumerable<float> Y = _getResultsOfFunction(_getY, yArgMin, yArgMax, pointsNum);

            IEnumerable<float> Z = _getResultsOfFunction(_getZ, zArgMin, zArgMax, pointsNum);

            return Z.Zip(X.Zip(Y, (x, y) => new Vector2(x, y)),
                (z, v) => new Vector3(v.X, v.Y, z));
        }
        

        private IEnumerable<float> _getResultsOfFunction(Func<float, float> func, float argMin, float argMax, int count)
        {
            return Enumerable.Range(0, count)
                .Select(i => argMin + i * _getIncrement(argMin, argMax, count))
                .Select(a => func(a));
        }

        private float _getIncrement (float min, float max, int count)
        {
            return (max - min) / (count - 1);
        }
    }
}
