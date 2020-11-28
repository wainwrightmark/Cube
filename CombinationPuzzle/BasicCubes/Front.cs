using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    public sealed class Front : ImmutableCubieCube
    {
        private Front()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new Front();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(
            Corner.Ufl,
            Corner.Dlf,
            Corner.Ulb,
            Corner.Ubr,
            Corner.Urf,
            Corner.Dfr,
            Corner.Dbl,
            Corner.Drb
        );

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations { get; } = ImmutableArray.Create(
            1,
            2,
            0,
            0,
            2,
            1,
            0,
            0
        );

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } = ImmutableArray.Create(
            Edge.Ur,
            Edge.Fl,
            Edge.Ul,
            Edge.Ub,
            Edge.Dr,
            Edge.Fr,
            Edge.Dl,
            Edge.Db,
            Edge.Uf,
            Edge.Df,
            Edge.Bl,
            Edge.Br
        );

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations { get; } = ImmutableArray.Create(
            0,
            1,
            0,
            0,
            0,
            1,
            0,
            0,
            1,
            1,
            0,
            0
        );

        /// <inheritdoc />
        public override string Name => nameof(Front);
    }
}