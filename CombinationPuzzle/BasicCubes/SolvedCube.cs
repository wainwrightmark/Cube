using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    public class SolvedCube : ImmutableCubieCube
    {
        private SolvedCube()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new SolvedCube();

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations => ZeroCornerOrientation;

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions => Extensions.GetEnumValues<Edge>().ToImmutableArray();

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations => ZeroEdgeOrientation;

        /// <inheritdoc />
        public override string Name => "Solved Cube";

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions => Extensions.GetEnumValues<Corner>().ToImmutableArray();
    }
}