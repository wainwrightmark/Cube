using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    public sealed class Left : ImmutableCubieCube
    {
        private Left()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new Left();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(
            Corner.Urf,
            Corner.Ulb,
            Corner.Dbl,
            Corner.Ubr,
            Corner.Dfr,
            Corner.Ufl,
            Corner.Dlf,
            Corner.Drb
        );

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations { get; } = ImmutableArray.Create(
            0,
            1,
            2,
            0,
            0,
            2,
            1,
            0
        );

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } = ImmutableArray.Create(
            Edge.Ur,
            Edge.Uf,
            Edge.Bl,
            Edge.Ub,
            Edge.Dr,
            Edge.Df,
            Edge.Fl,
            Edge.Db,
            Edge.Fr,
            Edge.Ul,
            Edge.Dl,
            Edge.Br
        );

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations => ZeroEdgeOrientation;

        /// <inheritdoc />
        public override string Name => nameof(Left);
    }
}