using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    public sealed class Up : ImmutableCubieCube
    {
        private Up()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new Up();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(
            Corner.Ubr,
            Corner.Urf,
            Corner.Ufl,
            Corner.Ulb,
            Corner.Dfr,
            Corner.Dlf,
            Corner.Dbl,
            Corner.Drb

            );

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations => ZeroCornerOrientation;

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } = ImmutableArray.Create
        (
            Edge.Ub,
            Edge.Ur,
            Edge.Uf,
            Edge.Ul,
            Edge.Dr,
            Edge.Df,
            Edge.Dl,
            Edge.Db,
            Edge.Fr,
            Edge.Fl,
            Edge.Bl,
            Edge.Br
        );

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations => ZeroEdgeOrientation;

        /// <inheritdoc />
        public override string Name => nameof(Up);
    }
}