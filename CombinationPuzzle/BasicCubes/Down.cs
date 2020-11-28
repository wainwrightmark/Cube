using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    public sealed class Down : ImmutableCubieCube
    {
        private Down()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new Down();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(Corner.Urf, Corner.Ufl, Corner.Ulb, Corner.Ubr, Corner.Dlf, Corner.Dbl, Corner.Drb, Corner.Dfr);

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations => ZeroCornerOrientation;

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } = ImmutableArray.Create(Edge.Ur, Edge.Uf, Edge.Ul, Edge.Ub, Edge.Df, Edge.Dl, Edge.Db, Edge.Dr, Edge.Fr, Edge.Fl, Edge.Bl, Edge.Br);

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations => ZeroEdgeOrientation;

        /// <inheritdoc />
        public override string Name => nameof(Down);
    }
}