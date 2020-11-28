using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    /// <summary>
    /// 120° clockwise rotation around the long diagonal URF-DBL
    /// </summary>
    public class Urf3Symmetry : ImmutableCubieCube
    {
        private Urf3Symmetry()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new Urf3Symmetry();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(Corner.Urf, Corner.Dfr, Corner.Dlf, Corner.Ufl, Corner.Ubr, Corner.Drb, Corner.Dbl, Corner.Ulb);

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations { get; } =ImmutableArray.Create(1, 2, 1, 2, 2, 1, 2, 1);

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } =ImmutableArray.Create(Edge.Uf, Edge.Fr, Edge.Df, Edge.Fl, Edge.Ub, Edge.Br, Edge.Db, Edge.Bl, Edge.Ur, Edge.Dr, Edge.Dl, Edge.Ul);

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations { get; } =ImmutableArray.Create(1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1);


        /// <inheritdoc />
        public override string Name => nameof(Urf3Symmetry);
    }
}