using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    /// <summary>
    /// 90° clockwise rotation around the axis through the U and D centers
    /// </summary>
    public class U4Symmetry : ImmutableCubieCube
    {
        private U4Symmetry()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new U4Symmetry();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(Corner.Ubr, Corner.Urf, Corner.Ufl, Corner.Ulb, Corner.Drb, Corner.Dfr, Corner.Dlf, Corner.Dbl);

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations => ZeroCornerOrientation;

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } =ImmutableArray.Create(Edge.Ub, Edge.Ur, Edge.Uf, Edge.Ul, Edge.Db, Edge.Dr, Edge.Df, Edge.Dl, Edge.Br, Edge.Fr, Edge.Fl, Edge.Bl);

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations { get; } = ImmutableArray.Create(0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1);

        /// <inheritdoc />
        public override string Name => nameof(U4Symmetry);
    }
}