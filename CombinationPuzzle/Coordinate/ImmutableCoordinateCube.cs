using System;
using CombinationPuzzle.Data;
using static CombinationPuzzle.Definitions;


namespace CombinationPuzzle.Coordinate
{
    /// <summary>
    /// Represents a cube on the coordinate level.
    /// In phase 1 a state is uniquely determined by the three coordinates flip, twist and slice.
    /// In phase 2 a state is uniquely determined by the three coordinates corners, ud_edges and slice_sorted.
    /// </summary>
    public class ImmutableCoordinateCube {

        private const int SolvedValue = 0;

        public readonly ushort Flip;
        public readonly ushort Twist;
        public readonly ushort SliceSorted;

        public readonly ushort Corners;
        public readonly ushort UEdges;
        public readonly ushort DEdges;

        /// <summary>
        /// Phase 1 Depth
        /// </summary>
        public readonly uint FlipsliceTwistDepthMod3; //TODO bit

        public readonly ushort? CornsliceDepth; //TODO bit

        /// <summary>
        /// Phase 2 Depth
        /// </summary>
        public readonly uint? CornersUdEdgesDepthMod3; //TODO bit

        public readonly Phase CurrentPhase;

        public ImmutableCoordinateCube(ushort flip, ushort twist, ushort sliceSorted, ushort corners, ushort uEdges, ushort dEdges, DataSource dataSource)
        {
            Flip = flip;
            Twist = twist;
            SliceSorted = sliceSorted;
            Corners = corners;
            UEdges = uEdges;
            DEdges = dEdges;

            var slice = (ushort) (SliceSorted / NPerm4);


            if (Flip != SolvedValue || slice != SolvedValue || Twist != SolvedValue)
            {
                CurrentPhase = Phase.Phase1;
                FlipsliceTwistDepthMod3 = dataSource.GetFlipsliceTwistDepthMod3(sliceSorted, flip, twist);
            }
            else
            {
                var udEdges = dataSource.GetUdEdges(UEdges, DEdges);
                CornersUdEdgesDepthMod3 =  dataSource.GetCornersUdEdgesDepth3(Corners, udEdges);
                CornsliceDepth =dataSource.GetCornsliceDepth(Corners, SliceSorted);

                if (Corners == SolvedValue && SliceSorted == SolvedValue && udEdges == SolvedValue)
                    CurrentPhase = Phase.Solved;
                else CurrentPhase = Phase.Phase2;
            }
        }


        public override string ToString() => (Flip, Twist, SliceSorted, Corners, UEdges, DEdges).ToString();

        public ImmutableCoordinateCube Move(Move move, DataSource dataSource)
        {
            var flip = dataSource.FlipMove[NMove * Flip + (int) move];
            var twist = dataSource.TwistMove[NMove * Twist + (int) move];
            var sliceSorted = dataSource.SliceSortedMove[NMove * SliceSorted + (int) move];
            var corners = dataSource.CornersMove[NMove * Corners + (int) move];
            var uEdges = dataSource.UEdgesMove[NMove * UEdges + (int) move];
            var dEdges = dataSource.DEdgesMove[NMove * DEdges + (int) move];


            return new ImmutableCoordinateCube(flip, twist, sliceSorted, corners, uEdges, dEdges, dataSource);
        }

        //public int GetPhase1Depth()
        //{
        //    if (CurrentPhase != Phase.Phase1)
        //        return 0;

        //    uint nextDepthMod3;
        //    if (FlipsliceTwistDepthMod3.Value == 0) nextDepthMod3 = 2;
        //    else nextDepthMod3 = FlipsliceTwistDepthMod3.Value - 1;


        //    var nextCube = Extensions.GetEnumValues<Move>()
        //        .Select(Move)
        //        .FirstOrDefault(x => x.FlipsliceTwistDepthMod3.Value == nextDepthMod3);

        //    if(nextCube == null)
        //        throw new Exception("No next move found");

        //    return 1 + nextCube.GetPhase1Depth();
        //}

        //public int? GetPhase2Depth()
        //{
        //    if (CurrentPhase == Phase.Phase1) return null;

        //    if(CurrentPhase == Phase.Solved)
        //        return 0; //Solved cube

        //    if (CornersUdEdgesDepthMod3.Value == 3) // unfilled entry, depth >= 11
        //        return 11;

        //    uint nextDepthMod3;
        //    if (FlipsliceTwistDepthMod3.Value == 0) nextDepthMod3 = 2;
        //    else nextDepthMod3 = FlipsliceTwistDepthMod3.Value - 1;

        //    var nextCube = MoveExtensions.Phase2MoveEnums
        //        .Select(Move)
        //        .FirstOrDefault(x => x.CornersUdEdgesDepthMod3.Value == nextDepthMod3);

        //    if (nextCube == null)
        //        throw new Exception("No next move found");

        //    return 1 + nextCube.GetPhase2Depth();
        //}



        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Flip, Twist, SliceSorted, Corners, UEdges, DEdges);

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is ImmutableCoordinateCube cc && Flip == cc.Flip && Twist == cc.Twist && SliceSorted == cc.SliceSorted &&
                   Corners == cc.SliceSorted && UEdges == cc.UEdges && DEdges == cc.DEdges;
        }
    }

    public enum Phase
    {
        Phase1 = 1,
        Phase2 = 2,
        Solved = 3
    }
}