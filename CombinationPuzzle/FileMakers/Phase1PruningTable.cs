using System;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Symmetries;
using static CombinationPuzzle.Definitions;

namespace CombinationPuzzle.FileMakers
{
    public sealed class Phase1PruningTable : FileMaker<uint>
    {
        private Phase1PruningTable()
        {
        }

        public static Phase1PruningTable Instance { get; } = new Phase1PruningTable();

        /// <inheritdoc />
        public override string FileName => "phase1_prun";


        public static uint GetFlipsliceTwistDepthMod3(ushort sliceSorted, ushort flip, ushort twist)
        {
            var slice = sliceSorted / NPerm4;
            var flipslice = NFlip * slice + flip;
            var classIndex = FlipSlice.FlipsliceClassidx[flipslice];
            var flipSliceSym = FlipSlice.FlipsliceSym[flipslice];

            var twistConj = ConjTwist.GetTwistConj(twist, flipSliceSym);

            var ix = Convert.ToInt32(NTwist * classIndex + twistConj);

            var depthMod3 = get_flipslice_twist_depth3(ix, Instance.Data);

            return depthMod3;
        }



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





        public override uint[] Create()
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

                var rep = FlipSlice.FlipsliceRep[i];
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
            // ##################################################################################################################

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
                        if (!backSearch && idx % 16 == 0 && table[idx / 16] == uint.MaxValue &&
                            twist < NTwist - 16)
                        {
                            twist += 16;
                            idx += 16;
                            continue;
                        }

                        //###################################################################################################
                        bool match;
                        if (backSearch)
                            match = get_flipslice_twist_depth3(idx, table) == 3;
                        else
                            match = get_flipslice_twist_depth3(idx, table) == depth3;

                        if (match)
                        {
                            var flipslice = FlipSlice.FlipsliceRep[fsClassidx];
                            var flip = Convert.ToInt32(flipslice % 2048);
                            var slice = Convert.ToInt32(flipslice >> 11);
                            foreach (var m in Extensions.GetEnumValues<Move>())
                            {
                                var twist1 = SpecialMoves.TwistMove[18 * twist + (int)m];
                                var flip1 = SpecialMoves.FlipMove[18 * flip + (int)m];
                                var slice1 = SpecialMoves.SliceSortedMove[432 * slice + (int)m] / 24;
                                var flipslice1 = (slice1 << 11) + flip1;
                                var fs1Classidx = FlipSlice.FlipsliceClassidx[flipslice1];
                                var fs1Sym = FlipSlice.FlipsliceSym[flipslice1];
                                twist1 = ConjTwist.GetTwistConj(twist1, fs1Sym);
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
                                                    var twist2 = ConjTwist.GetTwistConj(twist1, j);
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

            return table;
        }


    }
}
