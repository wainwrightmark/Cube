using System.Collections.Immutable;

namespace CombinationPuzzle.BasicCubes
{
    /// <summary>
    /// reflection at the plane through the U, D, FaceletPosition, B centers
    /// </summary>
    public class MirrorLr2Symmetry : ImmutableCubieCube
    {
        private MirrorLr2Symmetry()
        {
        }

        public static ImmutableCubieCube Instance { get; } = new MirrorLr2Symmetry();

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; } = ImmutableArray.Create(Corner.Ufl, Corner.Urf, Corner.Ubr, Corner.Ulb, Corner.Dlf, Corner.Dfr, Corner.Drb, Corner.Dbl);

        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations { get; } = ImmutableArray.Create(3, 3, 3, 3, 3, 3, 3, 3);

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; } = ImmutableArray.Create(Edge.Ul, Edge.Uf, Edge.Ur, Edge.Ub, Edge.Dl, Edge.Df, Edge.Dr, Edge.Db, Edge.Fl, Edge.Fr, Edge.Br, Edge.Bl);

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations => ZeroEdgeOrientation;


        /// <inheritdoc />
        public override string Name => nameof(MirrorLr2Symmetry);
    }
}