using System;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Symmetries;
using static CombinationPuzzle.Definitions;

namespace CombinationPuzzle.FileMakers
{
    /// <summary>
    /// The corners_ud_edges_depth3 pruning table for phase 2.
    /// </summary>
    public sealed class Phase2PruningTable : FileMaker<uint>
    {
        private Phase2PruningTable()
        {
        }

        public static FileMaker<uint> Instance { get; } = new Phase2PruningTable();


        public static uint GetCornersUdEdgesDepth3(ushort corners, ushort udEdges)
        {
            var cornerClassIndex = CornerSymmetries.CornerClassidx[corners];
            var cornerSym = CornerSymmetries.CornerSym[corners];

            var index = NUdEdges * cornerClassIndex + UdEdges.GetUdEdgesConj(udEdges, cornerSym);

            var r = get_corners_ud_edges_depth3(index, Instance.Data);

            return r;
        }


        // corners_ud_edges_depth3(ix) is *at least* the number of SpecialMoves % 3 to Solve phase 2 of a cube with index ix
        private static uint get_corners_ud_edges_depth3(int ix, uint[] array)
        {
            var y = array[ix / 16];
            y >>= ix % 16 * 2;
            return y & 3;
        }


        private static void set_corners_ud_edges_depth3(int ix, uint value, uint[] array)
        {
            var shift = ix % 16 * 2;
            var @base = ix >> 4;
            array[@base] &= ~(3U << shift) & uint.MaxValue;
            array[@base] |= value << shift;
        }


        /// <inheritdoc />
        public override string FileName => "phase2_prun";

        /// <summary>
        /// Creates the new table in memory
        /// </summary>
        /// <returns></returns>
        public override uint[] Create()
    {
        const int total = NCornersClass * NUdEdges;
        var table = new uint[total / 16];
        Array.Fill(table, uint.MaxValue);

        // ##################### create table with the symmetries of the corners classes ################################
        var cc = SolvedCube.Instance.Clone();
        var cSym = new ushort[NCornersClass];

        for(var i = 0; i < 0+NCornersClass; i++)
        {
            if ((i + 1) % 1000 == 0) Console.Write(".");
            var rep = CornerSymmetries .CornerRep[i];
            cc.set_corners(rep);

            for (byte s = 0; s < NSymD4H; s++)
            {
                var ss = Basic.Cubes[s].Clone();
                ss.CornerMultiply(cc);
                ss.CornerMultiply(Inverse.GetCube(s));
                if (ss.get_corners() == rep)
                {
                    const ushort u = 1;
                    var q = Convert.ToUInt16(u << s);
                    cSym[i] |= q;
                }
            }
        }

        Console.WriteLine();
        //###############################################################################################################
        var udEdge = 0;
        set_corners_ud_edges_depth3(NUdEdges * 0 + udEdge, 0, table);
        var done = 1;
        uint depth = 0;
        Console.WriteLine($"depth: {depth} done: {done}/{total}");
        while (depth < 10)
        {
            // we fill the table only do depth 9 + 1
            var depth3 = depth % 3;
            var idx = 0;
            var mult = 2;
            if (depth > 9) mult = 1;

            for (var cClassIndex = 0; cClassIndex < NCornersClass; cClassIndex++)
            {
                {
                    if ((cClassIndex + 1) % (20 * mult) == 0) Console.Write(".");

                    if ((cClassIndex + 1) % (1600 * mult) == 0) Console.WriteLine("");

                    udEdge = 0;
                    while (udEdge < NUdEdges)
                    {
                        // ################ if table entries are not populated, this is very fast: ##########################
                        if (idx % 16 == 0 && table[idx / 16] == uint.MaxValue && udEdge < NUdEdges - 16)
                        {
                            udEdge += 16;
                            idx += 16;
                            continue;
                        }

                        //###################################################################################################
                        if (get_corners_ud_edges_depth3(idx, table) == depth3)
                        {
                            var corner = CornerSymmetries .CornerRep[cClassIndex];
                            // only iterate phase 2 SpecialMoves
                            foreach (var m in Phase2MoveEnums)
                            {
                                var udEdge1 = SpecialMoves.UdEdgesMove[18 * udEdge + (int) m];
                                var corner1 = SpecialMoves.CornersMove[18 * corner + (int) m];
                                var c1ClassIndex = CornerSymmetries.CornerClassidx[corner1];
                                var c1Sym = CornerSymmetries.CornerSym[corner1];
                                udEdge1 = UdEdges.GetUdEdgesConj(udEdge1, c1Sym);// Symmetries.UdEdgesConjugation.UdEdgesConj[(udEdge1 << 4) + c1Sym];
                                var idx1 = 40320 * c1ClassIndex + udEdge1;
                                if (get_corners_ud_edges_depth3(idx1, table) == 3)
                                {
                                    // entry not yet filled
                                    set_corners_ud_edges_depth3(idx1, (depth + 1) % 3, table);
                                    done += 1;
                                    // ######symmetric position has eventually more than one representation #############
                                    var sym = cSym[c1ClassIndex];
                                    if (sym != 1)
                                    {
                                        for (byte j = 1; j < 16; j++)
                                        {
                                            sym >>= 1;
                                            if (sym % 2 == 1)
                                            {
                                                var udEdge2 = UdEdges.GetUdEdgesConj(udEdge1, j);
                                                // c1_classidx does not change
                                                var idx2 = 40320 * c1ClassIndex + udEdge2;
                                                if (get_corners_ud_edges_depth3(idx2, table) == 3)
                                                {
                                                    set_corners_ud_edges_depth3(idx2, (depth + 1) % 3, table);
                                                    done += 1;
                                                    //###################################################################################
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        udEdge += 1;
                        idx += 1;
                    }
                }
            }

            depth += 1;
            Console.WriteLine();
            Console.WriteLine($"depth: {depth} done: {done}/{total}");
        }

        return table;
    }


    public static readonly Move[] Phase2MoveEnums = {Move.U1, Move.U2, Move.U3, Move.R2, Move.F2, Move.D1, Move.D2, Move.D3, Move.L2, Move.B2};


    }
}
