using System.Collections.Generic;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;

namespace CombinationPuzzle.Symmetries
{
    /// <summary>
    /// Represents the 48 Cube Symmetries
    /// </summary>
    public static class Basic
    {
        public static readonly IReadOnlyDictionary<BasicSymmetry, ICubieCube> CubesByBasicSymmetry = new Dictionary<BasicSymmetry, ICubieCube>
        {
            {BasicSymmetry.RotateUrf3, Urf3Symmetry.Instance},
            {BasicSymmetry.RotateF2, F2Symmetry.Instance},
            {BasicSymmetry.RotateU4, U4Symmetry.Instance},
            {BasicSymmetry.MirrorLr2, MirrorLr2Symmetry.Instance},
        };


        private static IReadOnlyList<MutableCubieCube> Generate()
        {
            var cubeList = new List<MutableCubieCube>();
            var cc = SolvedCube.Instance.Clone();
            for (var urf3 = 0; urf3 < 3; urf3++)
            {
                for (var f2 = 0; f2 < 2; f2++)
                {
                    for (var u4 = 0; u4 < 4; u4++)
                    {
                        for (var lr2 = 0; lr2 < 2; lr2++)
                        {
                            cubeList.Add(cc.Clone());
                            cc.Multiply(CubesByBasicSymmetry[BasicSymmetry.MirrorLr2]);
                        }
                        cc.Multiply(CubesByBasicSymmetry[BasicSymmetry.RotateU4]);
                    }
                    cc.Multiply(CubesByBasicSymmetry[BasicSymmetry.RotateF2]);
                }
                cc.Multiply(CubesByBasicSymmetry[BasicSymmetry.RotateUrf3]);
            }

            return cubeList;
        }

        public static readonly IReadOnlyList<ICubieCube> Cubes = Generate();
    }
}
