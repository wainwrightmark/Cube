using CombinationPuzzle.Cubie;

namespace CombinationPuzzle.Symmetries
{
    /// <summary>
    /// Indices for the inverse symmetries: Basic[inv_idx[idx]] == Basic[idx]^(-1)
    /// </summary>
    public static class Inverse
    {
        private static int[] Generate()
        {
            var inverseSymmetriesIndexes = new int[Definitions.NSym];
            for (var j = 0; j < 0 + Definitions.NSym; j++)
            {
                for (var i = 0; i < 0 + Definitions.NSym; i++)
                {
                    var cc = Basic.Cubes[j].Clone();
                    cc.CornerMultiply(Basic.Cubes[i]);
                    if (cc.CornerPositions[(int) Corner.Urf] == Corner.Urf && cc.CornerPositions[(int)Corner.Ufl] == Corner.Ufl && cc.CornerPositions[(int)Corner.Ulb] == Corner.Ulb)
                    {
                        inverseSymmetriesIndexes[j] = i;
                        break;
                    }
                }
            }

            return inverseSymmetriesIndexes;
        }

        private static readonly int[] InvIDX = Generate();

        public static ICubieCube GetCube(int symmetry) => Basic.Cubes[InvIDX[symmetry]];
    }
}