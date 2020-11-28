using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    /// <summary>
    /// 180° rotation around the axis through the FaceletPosition and B centers
    /// </summary>
    public class F2Symmetry : ImmutableCubieCube
    {
        private F2Symmetry()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new F2Symmetry();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(Corner.Dlf, Corner.Dfr, Corner.Drb, Corner.Dbl, Corner.Ufl, Corner.Urf, Corner.Ubr, Corner.Ulb);

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations => ZeroCornerOrientation;

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } = ImmutableArray.Create(Edge.Dl, Edge.Df, Edge.Dr, Edge.Db, Edge.Ul, Edge.Uf, Edge.Ur, Edge.Ub, Edge.Fl, Edge.Fr, Edge.Br, Edge.Bl);

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations => ZeroEdgeOrientation;

        /// <inheritdoc />
        public override string Name => nameof(F2Symmetry);
    }
}