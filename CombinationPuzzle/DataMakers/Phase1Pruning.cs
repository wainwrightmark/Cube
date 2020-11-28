using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Data;
using CombinationPuzzle.Symmetries;
using static CombinationPuzzle.Definitions;

namespace CombinationPuzzle.DataMakers
{

    public static class CornsliceDepth
    {
        /// <summary>
        /// Create/load the cornslice_depth pruning table for phase 2. With this table we do a fast precheck at the beginning of phase 2.
        /// </summary>
        /// <returns></returns>
        public static ImmutableArray<byte> Create(DataSource dataSource)
        {
            var table = new byte[NCorners * NPerm4];
            Array.Fill(table, byte.MaxValue);

            table[0] = 0;
            var done = 1;
            byte depth = 0;
            while (done != NCorners * NPerm4)
            {
                for (var corners = 0; corners < NCorners; corners++)
                    for (var slice = 0; slice < NPerm4; slice++)
                        if (table[NPerm4 * corners + slice] == depth)
                            foreach (var m in CornsliceMoves)
                            {
                                var corners1 = dataSource.CornersMove[18 * corners + (int)m];
                                var slice1 = dataSource.SliceSortedMove[18 * slice + (int)m];
                                var idx1 = NPerm4 * corners1 + slice1;
                                if (table[idx1] != byte.MaxValue) continue; // entry not yet filled

                                table[idx1] = Convert.ToByte(depth + 1);
                                done += 1;
                                if (done % 20000 == 0) Console.Write(".");
                            }

                depth += 1;
            }

            return table.ToImmutableArray();
        }

        private static readonly Move[] CornsliceMoves = {Move.U1, Move.U2, Move.U3, Move.R2, Move.F2, Move.D1, Move.D2, Move.D3, Move.L2, Move.B2};
    }

    public static class UpDownEdgesConjugation
    {

        public static ImmutableArray<ushort> Create()
        {
            var udEdgesConj = new ushort[NUdEdges * NSymD4H];
            for (var t = 0; t < 0 + NUdEdges; t++)
            {
                if ((t + 1) % 400 == 0) Console.Write(".");
                if ((t + 1) % 32000 == 0) Console.WriteLine("");
                var cc = SolvedCube.Instance.Clone();

                cc.set_ud_edges(t);
                for (var s = 0; s < 0 + NSymD4H; s++)
                {
                    var ss = Basic.Cubes[s].Clone();
                    ss.EdgeMultiply(cc);
                    ss.EdgeMultiply(Inverse.GetCube(s));
                    udEdgesConj[NSymD4H * t + s] = Convert.ToUInt16(ss.get_ud_edges()); //TODO remove all these
                }
            }

            return udEdgesConj.ToImmutableArray();
        }

    }



    public static class Phase2Pruning
    {

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


        public static ImmutableArray<uint> Create(DataSource dataSource)
        {
            const int total = NCornersClass * NUdEdges;
            var table = new uint[total / 16];
            Array.Fill(table, uint.MaxValue);

            // ##################### create table with the symmetries of the corners classes ################################
            var cc = SolvedCube.Instance.Clone();
            var cSym = new ushort[NCornersClass];

            for (var i = 0; i < 0 + NCornersClass; i++)
            {
                if ((i + 1) % 1000 == 0) Console.Write(".");
                var rep = dataSource.CornerRep[i];
                cc.set_corners(rep);

                for (byte s = 0; s < NSymD4H; s++)
                {
                    var ss = Basic.Cubes[s].Clone();
                    ss.CornerMultiply(cc);
                    ss.CornerMultiply(Inverse.GetCube(s));
                    if (ss.GetCorners() == rep)
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
                                var corner = dataSource.CornerRep[cClassIndex];
                                // only iterate phase 2 SpecialMoves
                                foreach (var m in MoveExtensions.Phase2MoveEnums)
                                {
                                    var udEdge1 = dataSource.UdEdgesMove[18 * udEdge + (int) m];
                                    var corner1 = dataSource.CornersMove[18 * corner + (int) m];
                                    var c1ClassIndex = dataSource.CornerClassIndex[corner1];
                                    var c1Sym = dataSource.CornerSymmetry[corner1];
                                    udEdge1 = dataSource.GetUdEdgesConj(udEdge1,
                                        c1Sym); // Symmetries.UdEdgesConjugation.UdEdgesConj[(udEdge1 << 4) + c1Sym];
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
                                                    var udEdge2 = dataSource.GetUdEdgesConj(udEdge1, j);
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

            return table.ToImmutableArray();
        }
    }

    public static class Phase2EdgeMerge
    {
        private static readonly ImmutableHashSet<Edge> UEdges = new List<Edge>
        {
            Edge.Ur, Edge.Uf, Edge.Ul, Edge.Ub
        }.ToImmutableHashSet();

        private static readonly ImmutableHashSet<Edge> DEdges = new List<Edge>
        {
            Edge.Dr, Edge.Df, Edge.Dl, Edge.Db
        }.ToImmutableHashSet();

        private static readonly ImmutableArray<Edge> UdEdges = new List<Edge>
        {
            Edge.Ur, Edge.Uf, Edge.Ul, Edge.Ub, Edge.Dr, Edge.Df, Edge.Dl, Edge.Db
        }.ToImmutableArray();

        public static ImmutableArray<ushort> Create()
        {
            var cU = SolvedCube.Instance.Clone();
            var cD = SolvedCube.Instance.Clone();
            var cUdEdgePositions = SolvedCube.Instance.EdgePositions.ToArray();

            var cnt = 0;
            var table = new ushort[NUEdgesPhase2 * NPerm4];

            for (var i = 0; i < NUEdgesPhase2; i++)
            {
                cU.set_u_edges(i);
                for (var j = 0; j < NChoose84; j++)
                {
                    cD.set_d_edges(j * NPerm4);
                    var invalid = false;
                    foreach (var e in UdEdges.Cast<int>())
                    {
                        Edge position;
                        if (UEdges.Contains(cU.EdgePositions[e]))
                            position = cU.EdgePositions[e];
                        else if (DEdges.Contains(cD.EdgePositions[e]))
                            position = cD.EdgePositions[e];
                        else
                        {
                            invalid = true;
                            break;
                        }

                        cUdEdgePositions[e] = position;
                    } //TODO not sure why this is necessary

                    if (invalid) continue;

                    for (var k = 0; k < NPerm4; k++)
                    {
                        cD.set_d_edges(j * NPerm4 + k);
                        foreach (var e in UdEdges.Cast<int>())
                        {
                            if (UEdges.Contains(cU.EdgePositions[e])) cUdEdgePositions[e] = cU.EdgePositions[e];

                            else if (DEdges.Contains(cD.EdgePositions[e])) cUdEdgePositions[e] = cD.EdgePositions[e];
                        }

                        var cube = new MutableCubieCube(SolvedCube.Instance.CornerPositions.ToArray(), SolvedCube.Instance.CornerOrientations.ToArray(), cUdEdgePositions, SolvedCube.Instance.EdgeOrientations.ToArray());

                        table[NPerm4 * i + k] = cube.get_ud_edges();
                        cnt += 1;
                        if (cnt % 2000 == 0) Console.Write(".");
                    }
                }
            }

            return table.ToImmutableArray();
        }
    }

    public static class Phase1Pruning
    {
        // ####################### functions to extract or set values in the pruning tables #####################################
        // get_fst_depth3(ix) is *exactly* the number of SpecialMoves % 3 to Solve phase 1 of a cube with index ix
        private static uint get_flipslice_twist_depth3(int ix, uint[] table)
        {
            var y = table[ix / 16];
            y >>= ix % 16 * 2;
            var r = y & 3;
            return r;
        }



        private static void set_flipslice_twist_depth3(int ix, uint value, uint[] table)
        {
            var shift = (ix % 16) * 2;
            var @base = ix >> 4;

            var q = ~(3U << shift) & uint.MaxValue;

            table[@base] &= q;
            table[@base] |= value << shift;
        }

        public static ImmutableArray<uint> Create(DataSource dataSource)
        {
            const int total = NFlipsliceClass * NTwist;

            var table = new uint[total / 16 + 1];
            Array.Fill(table, uint.MaxValue);

            // #################### create table with the symmetries of the flipslice classes ###############################
            var cc = SolvedCube.Instance.Clone();
            var fsSym = new int[NFlipsliceClass];


            for (var i = 0; i < NFlipsliceClass; i++)
            {
                if ((i + 1) % 1000 == 0) Console.Write(".");

                var rep = dataSource.FlipsliceRep[i];
                var repModFlip = Convert.ToInt32(rep % NFlip);

                cc.set_slice(Convert.ToUInt16(rep / NFlip));
                cc.set_flip(repModFlip);
                for(var s = 0; s < NSymD4H; s++)
                {
                    var ss = Basic.Cubes[s].Clone();
                    ss.EdgeMultiply(cc);
                    ss.EdgeMultiply(Inverse.GetCube(s));
                    if (ss.get_slice() == rep / NFlip && ss.get_flip() == repModFlip)
                        fsSym[i] |= 1 << s;
                }
            }

            Console.WriteLine();

            var twist = 0;
            set_flipslice_twist_depth3(NTwist * 0 + twist, 0, table);
            var done = 1;
            uint depth = 0;
            var backSearch = false;
            Console.WriteLine($"depth: {depth} done: {done}/{total}");
            while (done < total)
            {
                var depth3 = depth % 3;
                if (depth == 9)
                {
                    // backwards search is faster for depth >= 9
                    Console.WriteLine("flipping to backwards search...");
                    backSearch = true;
                }

                var mult = depth < 8 ? 5 : 1;

                var idx = 0;
                for (var fsClassidx = 0; fsClassidx < NFlipsliceClass; fsClassidx++)
                {
                    if ((fsClassidx + 1) % (200 * mult) == 0) Console.Write(".");
                    if ((fsClassidx + 1) % (16000 * mult) == 0) Console.WriteLine("");
                    twist = 0;
                    while (twist < NTwist)
                    {
                        // ########## if table entries are not populated, this is very fast: ################################
                        if (!backSearch && idx % 16 == 0 && table[idx / 16] == uint.MaxValue && twist < NTwist - 16)
                        {
                            twist += 16;
                            idx += 16;
                            continue;
                        }

                        bool match;
                        if (backSearch)
                            match = get_flipslice_twist_depth3(idx, table) == 3;
                        else
                            match = get_flipslice_twist_depth3(idx, table) == depth3;

                        if (match)
                        {
                            var flipslice = dataSource.FlipsliceRep[fsClassidx];
                            var flip = Convert.ToInt32(flipslice % NFlip);
                            var slice = Convert.ToInt32(flipslice >> 11);
                            foreach (var m in MoveExtensions.AllMoves)
                            {
                                var twist1 = dataSource.TwistMove[18 * twist + (int)m];
                                var flip1 = dataSource.FlipMove[18 * flip + (int)m];
                                var slice1 = dataSource.SliceSortedMove[432 * slice + (int)m] / 24;
                                var flipslice1 = (slice1 << 11) + flip1;
                                var fs1Classidx = dataSource.FlipsliceClassIndex[flipslice1];
                                var fs1Sym = dataSource.FlipsliceSymmetry[flipslice1];
                                twist1 = dataSource.GetTwistConj(twist1, fs1Sym);
                                var idx1 = Convert.ToInt32(2187 * fs1Classidx + twist1);
                                if (!backSearch)
                                {
                                    if (get_flipslice_twist_depth3(idx1, table) == 3)
                                    {
                                        // entry not yet filled
                                        set_flipslice_twist_depth3(idx1, (depth + 1) % 3, table);
                                        done += 1;
                                        // ####symmetric position has eventually more than one representation ###############
                                        var sym = fsSym[fs1Classidx];
                                        if (sym != 1)
                                        {
                                            for (byte j = 1; j < 16; j++)
                                            {
                                                sym >>= 1;
                                                if (sym % 2 == 1)
                                                {
                                                    var twist2 = dataSource.GetTwistConj(twist1, j);
                                                    // fs2_classidx = fs1_classidx due to symmetry
                                                    var idx2 = Convert.ToInt32(2187 * fs1Classidx + twist2);
                                                    if (get_flipslice_twist_depth3(idx2, table) == 3)
                                                    {
                                                        set_flipslice_twist_depth3(idx2, (depth + 1) % 3, table);
                                                        done += 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // backwards search
                                    if (get_flipslice_twist_depth3(idx1, table) == depth3)
                                    {
                                        set_flipslice_twist_depth3(idx, (depth + 1) % 3, table);
                                        done += 1; //this doesn't seem to be working
                                        break;
                                    }
                                }
                            }
                        }

                        twist += 1;
                        idx += 1;
                    }
                }

                depth += 1;
                Console.WriteLine();
                Console.WriteLine($"depth: {depth} done: {done}/{total}");
            }

            return table.ToImmutableArray();
        }


    }
}

