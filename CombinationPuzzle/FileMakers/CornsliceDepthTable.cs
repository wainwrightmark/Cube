using System;

namespace CombinationPuzzle.FileMakers
{
    public sealed class CornsliceDepthTable : FileMaker<byte>
    {
        private CornsliceDepthTable()
        {
        }

        public static FileMaker<byte> Instance { get; } = new CornsliceDepthTable();

        /// <inheritdoc />
        public override string FileName => "phase2_cornsliceprun";

        public static byte GetCornsliceDepth(ushort corners, ushort sliceSorted)
        {
            var index = 24 * corners + sliceSorted;

            var r = Instance.Data[index];

            return r;
        }


        /// <summary>
        /// Create/load the cornslice_depth pruning table for phase 2. With this table we do a fast precheck at the beginning of phase 2.
        /// </summary>
        /// <returns></returns>
        public override byte[] Create()
        {
            var table = new byte[Definitions.NCorners * Definitions.NPerm4];
            Array.Fill(table, byte.MaxValue);

            table[0] = 0;
            var done = 1;
            byte depth = 0;
            while (done != Definitions.NCorners * Definitions.NPerm4)
            {
                for (var corners = 0; corners < Definitions.NCorners; corners++)
                for (var slice = 0; slice < Definitions.NPerm4; slice++)
                    if (table[Definitions.NPerm4 * corners + slice] == depth)
                        foreach (var m in CornsliceMoves)
                        {
                            var corners1 = SpecialMoves.CornersMove[18 * corners + (int)m];
                            var slice1 = SpecialMoves.SliceSortedMove[18 * slice + (int)m];
                            var idx1 = Definitions.NPerm4 * corners1 + slice1;
                            if (table[idx1] != byte.MaxValue) continue; // entry not yet filled

                            table[idx1] = Convert.ToByte(depth + 1);
                            done += 1;
                            if (done % 20000 == 0) Console.Write(".");
                        }

                depth += 1;
            }

            return table;
        }


        private static readonly Move[] CornsliceMoves = {Move.U1, Move.U2, Move.U3, Move.R2, Move.F2, Move.D1, Move.D2, Move.D3, Move.L2, Move.B2};
    }
}