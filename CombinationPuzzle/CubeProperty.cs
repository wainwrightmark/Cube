using System.Collections.Generic;
using System.Collections.Immutable;
using CombinationPuzzle.Cubie;

namespace CombinationPuzzle
{

    public class SliceProperty : CubeProperty
    {
        private SliceProperty() { }

        public static CubeProperty Instance { get; } = new SliceProperty();

        /// <inheritdoc />
        public override bool IsEdges => true;

        /// <inheritdoc />
        public override void SetValue(MutableCubieCube cube, ushort newValue) => cube.set_slice(newValue);

        /// <inheritdoc />
        public override ushort GetValue(ICubieCube cube) => cube.get_slice();

        /// <inheritdoc />
        public override ushort MaxI => Definitions.NSlice;
    }

    public class FlipMoveProperty : CubeProperty
    {
        private FlipMoveProperty() { }

        public static CubeProperty Instance { get; } = new FlipMoveProperty();

        /// <inheritdoc />
        public override bool IsEdges => true;

        /// <inheritdoc />
        public override void SetValue(MutableCubieCube cube, ushort newValue) => cube.set_flip(newValue);

        /// <inheritdoc />
        public override ushort GetValue(ICubieCube cube) => cube.get_flip();

        /// <inheritdoc />
        public override ushort MaxI => Definitions.NFlip;
    }


    public class DEdgesMoveProperty : CubeProperty
    {
        private DEdgesMoveProperty()
        {
        }

        public static CubeProperty Instance { get; } = new DEdgesMoveProperty();

        /// <inheritdoc />
        public override bool IsEdges => true;

        /// <inheritdoc />
        public override void SetValue(MutableCubieCube cube, ushort newValue) => cube.set_d_edges(newValue);

        /// <inheritdoc />
        public override ushort GetValue(ICubieCube cube) => cube.get_d_edges();

        /// <inheritdoc />
        public override ushort MaxI => Definitions.NSliceSorted;
    }

    public class CornersMoveProperty : CubeProperty
    {
        private CornersMoveProperty()
        {
        }

        public static CubeProperty Instance { get; } = new CornersMoveProperty();

        /// <inheritdoc />
        public override bool IsEdges => false;

        /// <inheritdoc />
        public override void SetValue(MutableCubieCube cube, ushort newValue) => cube.set_corners(newValue);

        /// <inheritdoc />
        public override ushort GetValue(ICubieCube cube) => cube.GetCorners();

        /// <inheritdoc />
        public override ushort MaxI => Definitions.NCorners;
    }

    public class SliceSortedMoveProperty : CubeProperty
    {
        private SliceSortedMoveProperty()
        {
        }

        public static CubeProperty Instance { get; } = new SliceSortedMoveProperty();

        /// <inheritdoc />
        public override bool IsEdges => true;

        /// <inheritdoc />
        public override void SetValue(MutableCubieCube cube, ushort newValue) => cube.set_slice_sorted(newValue);

        /// <inheritdoc />
        public override ushort GetValue(ICubieCube cube) => cube.get_slice_sorted();

        /// <inheritdoc />
        public override ushort MaxI => Definitions.NSliceSorted;
    }

    public class TwistMoveProperty : CubeProperty
    {
        private TwistMoveProperty()
        {
        }

        public static CubeProperty Instance { get; } = new TwistMoveProperty();

        /// <inheritdoc />
        public override bool IsEdges => false;

        /// <inheritdoc />
        public override void SetValue(MutableCubieCube cube, ushort newValue) => cube.set_twist(newValue);

        /// <inheritdoc />
        public override ushort GetValue(ICubieCube cube) => cube.get_twist();

        /// <inheritdoc />
        public override ushort MaxI => Definitions.NTwist;
    }

    public class UDEdgesMoveProperty : CubeProperty
    {
        private UDEdgesMoveProperty()
        {
        }

        public static UDEdgesMoveProperty Instance { get; } = new UDEdgesMoveProperty();

        /// <inheritdoc />
        public override bool IsEdges { get; } = true;

        /// <inheritdoc />
        public override void SetValue(MutableCubieCube cube, ushort newValue) => cube.set_ud_edges(newValue);

        /// <inheritdoc />
        public override ushort GetValue(ICubieCube cube) => cube.get_ud_edges();

        /// <inheritdoc />
        public override ushort MaxI => Definitions.NUdEdges;

        /// <inheritdoc />
        public override ImmutableHashSet<Move>? MovesToIgnore { get; } = new List<Move>()
        {
            Move.R1,
            Move.R3,

            Move.F1,
            Move.F3,

            Move.L1,
            Move.L3,

            Move.B1,
            Move.B3
        }.ToImmutableHashSet();
    }

    public class UEdgesMovesProperty : CubeProperty
    {
        private UEdgesMovesProperty()
        {
        }

        public static CubeProperty Instance { get; } = new UEdgesMovesProperty();

        /// <inheritdoc />
        public override bool IsEdges { get; } = true;

        /// <inheritdoc />
        public override void SetValue(MutableCubieCube cube, ushort newValue) => cube.set_u_edges(newValue);

        /// <inheritdoc />
        public override ushort GetValue(ICubieCube cube) => cube.get_u_edges();

        /// <inheritdoc />
        public override ushort MaxI => Definitions.NSliceSorted;
    }

    public abstract class CubeProperty
    {
        /// <summary>
        /// If true this is an edge property. If false this is a corner property.
        /// </summary>
        public abstract bool IsEdges { get; }

        public abstract void SetValue(MutableCubieCube cube, ushort newValue);

        public abstract ushort GetValue(ICubieCube cube);

        /// <summary>
        /// The maximum different values for this property
        /// </summary>
        public abstract ushort MaxI { get; }

        /// <summary>
        /// Moves for which this property is undefined.
        /// </summary>
        public virtual ImmutableHashSet<Move>? MovesToIgnore => null;


        public static ImmutableList<CubeProperty> AllProperties { get; } =
            new List<CubeProperty>
            {
                SliceProperty.Instance,
                CornersMoveProperty.Instance,
                DEdgesMoveProperty.Instance,
                FlipMoveProperty.Instance,
                SliceSortedMoveProperty.Instance,
                TwistMoveProperty.Instance,
                UDEdgesMoveProperty.Instance
            }.ToImmutableList();

    }
}
