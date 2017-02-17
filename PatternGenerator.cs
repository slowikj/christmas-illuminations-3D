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
        private Func<float, float> _flatFunc, _spaceFunc;
        private float _spaceArgMin, _spaceArgMax;
        public PatternGenerator(Func<float, float> flatFunc, Func<float, float> spaceFunc, float spaceFuncMin, float spaceFuncMax)
        {
            _flatFunc = flatFunc;
            _spaceFunc = spaceFunc;

            _spaceArgMin = spaceFuncMin;
            _spaceArgMax = spaceFuncMax;
        }

        public IEnumerable<Vector3> GetPoints(float flatArgMin, float flatArgMax, int pointsNum)
        {
            IEnumerable<float> X = Enumerable.Range(0, pointsNum)
                .Select(i => flatArgMin + i * _getIncrement(flatArgMin, flatArgMax, pointsNum));

            IEnumerable<float> Y = _getResultsOfFunction(_spaceFunc, _spaceArgMin, _spaceArgMax, pointsNum);

            IEnumerable<float> Z = _getResultsOfFunction(_flatFunc, flatArgMin, flatArgMax, pointsNum);

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
