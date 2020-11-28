using System;
using System.Collections.Immutable;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;

namespace CombinationPuzzle.Symmetries
{
    /// <summary>
    /// Generate the tables to handle the symmetry reduced flip-slice coordinate in  phase 1
    /// </summary>
    public static class FlipSlice
    {

        public static (ImmutableArray<ushort> classIndex, ImmutableArray<byte> sym, ImmutableArray<uint> rep) Create()
        {
            var flipsliceClassidx = new ushort[Definitions.NFlip * Definitions.NSlice];
            var flipsliceSym = new byte[Definitions.NFlip * Definitions.NSlice];
            var flipsliceRep = new uint[Definitions.NFlipsliceClass];
            Array.Fill(flipsliceClassidx, ushort.MaxValue); //todo find a way around this

            //TODO check what actual types should be
            ushort classIndex = 0;
            var cc = SolvedCube.Instance.Clone();
            for (ushort slc = 0; slc < Definitions.NSlice; slc++)
            {
                cc.set_slice(slc);

                for (var flip = 0; flip < Definitions.NFlip; flip++)
                {
                    cc.set_flip(flip);
                    var idx = Convert.ToUInt32(Definitions.NFlip * slc + flip);
                    if ((idx + 1) % 4000 == 0) Console.Write(".");
                    if ((idx + 1) % 320000 == 0) Console.WriteLine("");
                    if (flipsliceClassidx[idx] != ushort.MaxValue)
                        continue;

                    flipsliceClassidx[idx] = classIndex;
                    flipsliceSym[idx] = 0;
                    flipsliceRep[classIndex] = idx;

                    for (byte s = 0; s < Definitions.NSymD4H; s++)
                    {
                        // conjugate represented by all 16 symmetries
                        var ss = Inverse.GetCube(s).Clone();
                        ss.EdgeMultiply(cc);
                        ss.EdgeMultiply(Basic.Cubes[s]);
                        var idxNew = Definitions.NFlip * ss.get_slice() + ss.get_flip();
                        if (flipsliceClassidx[idxNew] == ushort.MaxValue)
                        {
                            flipsliceClassidx[idxNew] = classIndex;
                            flipsliceSym[idxNew] = s;
                        }
                    }


                    classIndex += 1;
                }
            }

            return (flipsliceClassidx.ToImmutableArray(), flipsliceSym.ToImmutableArray(), flipsliceRep.ToImmutableArray());
        }



        //public static ushort[] FlipsliceClassIndex = FlipSliceClass.Instance.LoadOrCreate();
        //public static byte[] FlipsliceSym = FlipSliceSymmetry.Instance.LoadOrCreate();
        //public static uint[] FlipsliceRep = FlipSliceRep.Instance.LoadOrCreate();
    }
}