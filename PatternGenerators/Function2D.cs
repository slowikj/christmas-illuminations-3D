using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projekt4.PatternGenerators
{
    public class Function2D
    {
        private Func<float, Vector2> _getRes;
        private Func<Vector2, float> _getCartesian;
        private Range _argumentsRange;

        public Function2D(Func<float, float> getRes, Func<Vector2, float> getCartesian, Range argumentsRange)
        {
            _getRes = a => new Vector2(a, getRes(a));
            _getCartesian = getCartesian;
            _argumentsRange = argumentsRange;
        }

        public IEnumerable<float> GetResults(int numberOfPoints)
        {
            return _getResultsOfFunction(_getRes, _argumentsRange.Min, _argumentsRange.Max, numberOfPoints)
                .Select(r => _getCartesian(r));
        }

        private IEnumerable<Vector2> _getResultsOfFunction(Func<float, Vector2> func, float argMin, float argMax, int count)
        {
            return Enumerable.Range(0, count)
                .Select(i => argMin + i * _getIncrement(argMin, argMax, count))
                .Select(a => func(a));
        }

        private float _getIncrement(float min, float max, int count)
        {
            return (max - min) / (count - 1);
        }

    }
}
