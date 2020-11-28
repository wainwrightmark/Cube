using System.Linq;

namespace CombinationPuzzle.Symmetries
{
    /// <summary>
    /// Generate the table for the conjugation of a move m by a symmetry s. conj_move[m, s] = s*m*s^-1
    /// </summary>
    public static class ConjugationMoves
    {
        private static Move[,] Generate()
        {
            var conjMove = new Move[Definitions.NMove, Definitions.NSym];
            for (var s = 0; s < 0 + Definitions.NSym; s++)
                foreach (var m in Extensions.GetEnumValues<Move>())
                {
                    var ss = Basic.Cubes[s].Clone();
                    ss.Multiply(MoveCubes.BasicCubesByMove[m]);
                    ss.Multiply(Inverse.GetCube(s));
                    var move = Extensions.GetEnumValues<Move>().Single(m2 => ss.Equals(MoveCubes.BasicCubesByMove[m2]));
                    conjMove[(int)m, s] = move;
                }

            return conjMove;
        }

        public static readonly Move[,] ConjMove = Generate();
    }
}