using System;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using static CombinationPuzzle.Definitions;

namespace CombinationPuzzle.FileMakers
{
    /// <summary>
    /// Generate the phase 2 table for the conjugation of the URtoDB coordinate by a symmetrie
    /// </summary>
    public class UdEdges : FileMaker<ushort>
    {
        private UdEdges()
        {
        }

        public static ushort GetUdEdgesConj(ushort udEdges, byte cornerSym)
        {
            var idx = (udEdges << 4) + cornerSym;

            var r = Instance.Data[idx];

            return r;

        }

        public static FileMaker<ushort> Instance { get; } = new UdEdges();

        /// <inheritdoc />
        public override string FileName => "conj_ud_edges";

        /// <inheritdoc />
        public override ushort[] Create()
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
                    var ss = Symmetries.Basic.Cubes[s].Clone();
                    ss.EdgeMultiply(cc);
                    ss.EdgeMultiply(Symmetries.Inverse.GetCube(s));
                    udEdgesConj[NSymD4H * t + s] = Convert.ToUInt16(ss.get_ud_edges()); //TODO remove all these
                }
            }

            return udEdgesConj;
        }
    }
}
