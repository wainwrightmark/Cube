using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using CombinationPuzzle.Coordinate;
using Medallion.Collections;

namespace CombinationPuzzle.Solver
{
    public sealed class SearchState : IComparable<SearchState>
    {
        /// <summary>
        /// Whether to invert the cube before applying the two-phase-algorithm
        /// </summary>
        public readonly bool Invert;
        /// <summary>
        /// Rotates the  cube 120° * rotation along the long diagonal before applying the two-phase-algorithm
        /// </summary>
        public readonly byte Rotation;

        public readonly ImmutableCoordinateCube Cube;

        public readonly ImmutableLinkedList<Move> MovesSoFar;

        /// <summary>
        /// Once a cube is deepening, all moves must be deepening moves.
        /// </summary>
        public readonly bool Deepening;

        public int MovesDeep => MovesSoFar.Count;

        public readonly int SearchStatePriority;

        /// <inheritdoc />
        public override string ToString() => SearchStatePriority.ToString();

        public SearchState(bool invert, byte rotation, ImmutableCoordinateCube cube, ImmutableLinkedList<Move> movesSoFar, bool deepening)
        {
            Invert = invert;
            Rotation = rotation;
            Cube = cube;
            MovesSoFar = movesSoFar;
            Deepening = deepening;
            SearchStatePriority = GetSearchStatePriority(this);
        }




        public void Iterate(ISolveCoordinator solveCoordinator)
        {
            switch (Cube.CurrentPhase)
            {
                case Phase.Phase1:
                    IteratePhase1(solveCoordinator);
                    break;
                case Phase.Phase2:
                    IteratePhase2(solveCoordinator);
                    break;
                case Phase.Solved:
                    solveCoordinator.TryAddSolution(Solution.Create(MovesSoFar, Invert, Rotation));
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private void IteratePhase1(ISolveCoordinator solveCoordinator)
        {
            uint nextDepth;
            if (Cube.FlipsliceTwistDepthMod3 == 0) nextDepth = 2;
            else
                nextDepth = Cube.FlipsliceTwistDepthMod3 - 1;

            if (nextDepth + 1 + MovesDeep >= solveCoordinator.MaxTotalMoves)
                return;//No way to solve in time

            ImmutableArray<Move> moves;

            if (MovesSoFar.Any())
                moves = MoveExtensions.PossibleNextMovesPhase1[(int) MovesSoFar.Head];
            else moves = MoveExtensions.AllMoves;

            foreach (var moveEnum in moves)
            {
                var nextCube = Cube.Move(moveEnum, solveCoordinator.DataSource);

                var nextIsDeepening = nextCube.FlipsliceTwistDepthMod3 == nextDepth;

                if (nextIsDeepening || (!Deepening && MovesDeep <= 2 )) //Once we are deepening, we do not allow non-deepening moves
                {
                    var nextState = new SearchState(Invert, Rotation, nextCube, MovesSoFar.Prepend(moveEnum),
                        nextIsDeepening);

                    solveCoordinator.MaybeAddSearch(nextState);
                }
            }
        }

        private void IteratePhase2(ISolveCoordinator solveCoordinator)
        {
            var togo2Limit = Math.Min(solveCoordinator.MaxTotalMoves - MovesSoFar.Count, 11);

            if (Cube.CornsliceDepth!.Value >= togo2Limit)
                return; // this check speeds up the computation


            uint nextDepth;
            if (Cube.CornersUdEdgesDepthMod3!.Value == 0) nextDepth = 2;
            else
                nextDepth = Cube.CornersUdEdgesDepthMod3.Value - 1;

            ImmutableArray<Move> moves;

            if (MovesSoFar.Any())
                moves = MoveExtensions.PossibleNextMovesPhase2[(int)MovesSoFar.Head];
            else moves = MoveExtensions.Phase2MoveEnums;

            foreach (var moveEnum in moves)
            {
                var nextCube = Cube.Move(moveEnum, solveCoordinator.DataSource);

                var nextIsDeeper = nextCube.CornersUdEdgesDepthMod3!.Value == nextDepth;

                if (nextIsDeeper)
                {
                    var nextState = new SearchState(Invert, Rotation, nextCube, MovesSoFar.Prepend(moveEnum), nextIsDeeper);

                    solveCoordinator.MaybeAddSearch(nextState);
                }
            }
        }

        private static int GetSearchStatePriority(SearchState ss)
        {
            switch (ss.Cube.CurrentPhase)
            {
                case Phase.Phase1:
                    {
                        if (ss.MovesDeep == 0) return SearchStatePriorityValues.Phase1Depth0;

                        if (ss.Deepening)
                        {
                            if (ss.MovesDeep >= 12) return SearchStatePriorityValues.Phase1DeepeningDepth12Plus;
                            return (SearchStatePriorityValues.Phase1DeepeningDepth - ss.MovesDeep);
                        }
                        else
                        {
                            if (ss.MovesDeep >= 12) return SearchStatePriorityValues.Phase1FreeDepth12Plus;
                            return (SearchStatePriorityValues.Phase1FreeDepth12Plus - ss.MovesDeep);
                        }
                    }
                case Phase.Phase2:
                    {
                        if (ss.Cube.CornsliceDepth!.Value >= 12) return SearchStatePriorityValues.Phase2Cornslice12Plus;

                        return (SearchStatePriorityValues.Phase2Cornslice0 + ss.Cube.CornsliceDepth.Value);
                    }
                case Phase.Solved:
                    return SearchStatePriorityValues.Solved;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        /// <inheritdoc />
        public int CompareTo(SearchState? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            return other.SearchStatePriority.CompareTo(SearchStatePriority);
        }
    }
}