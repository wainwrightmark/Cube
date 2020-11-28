using System;
using System.Collections.Immutable;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;

namespace CombinationPuzzle.Symmetries
{
    /// <summary>
    /// Generate the tables to handle the symmetry reduced corner permutation coordinate in phase 2
    /// </summary>
    public static class CornerSymmetries
    {

        public static (ImmutableArray<ushort> corner_classidx, ImmutableArray<byte> corner_sym,ImmutableArray<ushort> corner_rep) Create()
        {
            ushort[] cornerRep;
            byte[] cornerSym;
            ushort[] cornerClassidx;

            cornerClassidx = new ushort[Definitions.NCorners];
            Array.Fill(cornerClassidx, ushort.MaxValue);
            cornerSym = new byte[Definitions.NCorners];
            cornerRep = new ushort[Definitions.NCornersClass];


            ushort classidx = 0;
            var cc = SolvedCube.Instance.Clone();
            for (ushort cp = 0; cp < Definitions.NCorners; cp++)
            {
                cc.set_corners(cp);
                if ((cp + 1) % 8000 == 0) Console.Write(".");
                if (cornerClassidx[cp] == ushort.MaxValue)
                {
                    cornerClassidx[cp] = classidx;
                    cornerSym[cp] = 0;
                    cornerRep[classidx] = cp;
                }
                else
                    continue;

                for (byte s = 0; s < Definitions.NSymD4H; s++)
                {
                    // conjugate represented by all 16 symmetries
                    var ss = Inverse.GetCube(s).Clone();
                    ss.CornerMultiply(cc);
                    ss.CornerMultiply(Basic.Cubes[s]); //s^-1*cc*s
                    var cpNew = ss.GetCorners();
                    if (cornerClassidx[cpNew] == ushort.MaxValue)
                    {
                        cornerClassidx[cpNew] = classidx;
                        cornerSym[cpNew] = s;
                    }
                }
                classidx += 1;
            }

            return (cornerClassidx.ToImmutableArray(), cornerSym.ToImmutableArray(), cornerRep.ToImmutableArray());
        }



        //public static readonly ushort[] CornerClassidx = FileMakers.CornerClassIndex.Instance.LoadOrCreate();
        //public static readonly byte[] CornerSym = FileMakers.CornerSymmetry.Instance.LoadOrCreate();
        //public static ushort[] CornerRep = FileMakers.CornerRep.Instance.LoadOrCreate();
    }
}