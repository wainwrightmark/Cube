using System;
using System.Collections.Generic;
using System.Linq;

namespace CombinationPuzzle.Cubie
{
    public class CubeComparer : IEqualityComparer<ICubieCube>
    {
        private CubeComparer()
        {
        }

        public static IEqualityComparer<ICubieCube> Instance { get; } = new CubeComparer();

        /// <inheritdoc />
        public bool Equals(ICubieCube? x, ICubieCube? y)
        {
            if (x == null || y == null) return false;


            return
                x.CornerOrientations.SequenceEqual(y.CornerOrientations) &&
                x.CornerPositions.SequenceEqual(y.CornerPositions) &&
                x.EdgeOrientations.SequenceEqual(y.EdgeOrientations) &&
                x.EdgePositions.SequenceEqual(y.EdgePositions);
        }

        /// <inheritdoc />
        public int GetHashCode(ICubieCube obj)
        {
            return HashCode.Combine(obj.CornerPositions[0], obj.EdgeOrientations[0], obj.CornerOrientations[0], obj.EdgePositions[0]);
        }
    }
}