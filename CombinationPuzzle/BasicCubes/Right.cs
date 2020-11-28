using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    public sealed class Right : ImmutableCubieCube
    {
        private Right()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new Right();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(
            Corner.Dfr,
            Corner.Ufl,
            Corner.Ulb,
            Corner.Urf,
            Corner.Drb,
            Corner.Dlf,
            Corner.Dbl,
            Corner.Ubr
        );

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations { get; } = ImmutableArray.Create(
            2,
            0,
            0,
            1,
            1,
            0,
            0,
            2
        );

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } = ImmutableArray.Create(
            Edge.Fr,
            Edge.Uf,
            Edge.Ul,
            Edge.Ub,
            Edge.Br,
            Edge.Df,
            Edge.Dl,
            Edge.Db,
            Edge.Dr,
            Edge.Fl,
            Edge.Bl,
            Edge.Ur
        );

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations => ZeroEdgeOrientation;

        /// <inheritdoc />
        public override string Name => nameof(Right);
    }
}