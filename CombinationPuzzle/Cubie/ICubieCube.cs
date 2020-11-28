using System.Collections.Generic;

namespace CombinationPuzzle.Cubie
{
    public interface ICubieCube
    {
        public MutableCubieCube Clone();

        public IReadOnlyList<int> CornerOrientations { get; }

        public IReadOnlyList<Corner> CornerPositions { get; }

        public IReadOnlyList<int> EdgeOrientations { get; }

        public IReadOnlyList<Edge> EdgePositions { get; }
    }
}