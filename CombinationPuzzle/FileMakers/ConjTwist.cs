using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using static CombinationPuzzle.Definitions;

namespace CombinationPuzzle.FileMakers
{
    /// <summary>
    /// Generate the phase 1 table for the conjugation of the twist t by a symmetry s. twist_conj[t, s] = s*t*s^-1
    /// </summary>
    public class ConjTwist : FileMaker<ushort>
    {
        private ConjTwist()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new ConjTwist();


        public static ushort GetTwistConj(ushort twist, byte flipSliceSym)
        {
            var r = Instance.Data[(twist << 4) + flipSliceSym];

            return r;
        }


        /// <inheritdoc />
        public override string FileName => "conj_twist";

        /// <inheritdoc />
        public override ushort[] Create()
        {
            var twistConj = new ushort[NTwist * NSymD4H];
            for (var t = 0; t < 0 + NTwist; t++)
            {
                var cc = SolvedCube.Instance.Clone();
                cc.set_twist(t);
                for (var s = 0; s < 0 + NSymD4H; s++)
                {
                    var ss = Symmetries.Basic.Cubes[s].Clone();
                    ss.CornerMultiply(cc);
                    ss.CornerMultiply(Symmetries.Inverse.GetCube(s));
                    twistConj[NSymD4H * t + s] = ss.get_twist();
                }
            }

            return twistConj;
        }
    }
}
