using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medallion.Collections;

namespace CombinationPuzzle.Solver
{
    public class Solution
    {
        public static Solution Create(ImmutableLinkedList<Move> mll, bool invert, byte rotation)
        {
            IEnumerable<Move> moves = mll;

            if (invert)
                moves = moves.Select(m => m.Inverse());
            else
                moves = moves.Reverse();

            var solutionMoves = moves
                .Select(m => Symmetries.ConjugationMoves.ConjMove[(int)m, 16 * rotation]).ToList();

            return new Solution(solutionMoves);
        }

        public Solution(IReadOnlyList<Move> solutionMoves)
        {
            SolutionMoves = solutionMoves;
        }

        public IReadOnlyList<Move> SolutionMoves { get; }

        public static string CreateName(IReadOnlyCollection<Move> moveEnums, Orientation orientation)
        {
            var sb = new StringBuilder();

            foreach (var m in moveEnums)
                sb.Append(m.GetDisplayName(orientation, true) + " ");

            sb.Append("(" + moveEnums.Count + " moves)");

            return sb.ToString();
        }

        public string GetName(Orientation orientation, bool inverse)
        {
            var moves = inverse ? SolutionMoves.Select(x => x.Inverse()).Reverse().ToList() : SolutionMoves;

            return CreateName(moves, orientation);
        }


        /// <inheritdoc />
        public override string ToString() => CreateName(SolutionMoves, Orientation.Default);
    }
}