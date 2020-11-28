using System.Collections.Immutable;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using static CombinationPuzzle.Definitions;

namespace CombinationPuzzle.DataMakers
{
    public static class TwistConjugation
    {
        public static ImmutableArray<ushort> CreateTable()
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

            return twistConj.ToImmutableArray();
        }

    }
}
