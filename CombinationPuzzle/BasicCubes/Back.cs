using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    public sealed class Back : ImmutableCubieCube
    {
        private Back()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new Back();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(
            Corner.Urf,
            Corner.Ufl,
            Corner.Ubr,
            Corner.Drb,
            Corner.Dfr,
            Corner.Dlf,
            Corner.Ulb,
            Corner.Dbl
        );

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations { get; } = ImmutableArray.Create(
            0,
            0,
            1,
            2,
            0,
            0,
            2,
            1
        );

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } = ImmutableArray.Create(
            Edge.Ur,
            Edge.Uf,
            Edge.Ul,
            Edge.Br,
            Edge.Dr,
            Edge.Df,
            Edge.Dl,
            Edge.Bl,
            Edge.Fr,
            Edge.Fl,
            Edge.Ub,
            Edge.Db
        );

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations { get; } = ImmutableArray.Create(
            0,
            0,
            0,
            1,
            0,
            0,
            0,
            1,
            0,
            0,
            1,
            1
        );

        /// <inheritdoc />
        public override string Name => nameof(Back);
    }
}