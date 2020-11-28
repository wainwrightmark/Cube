using System.Collections.Generic;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;

namespace CombinationPuzzle
{
    public static class MoveCubes
    {

        public static readonly IReadOnlyDictionary<FaceColor, ICubieCube> BasicCubesByColor =
            new Dictionary<FaceColor, ICubieCube>
            {
                {FaceColor.U, Up.Instance},
                {FaceColor.R, Right.Instance},
                {FaceColor.F, Front.Instance},
                {FaceColor.D, Down.Instance},
                {FaceColor.L, Left.Instance},
                {FaceColor.B, Back.Instance},
            };

        public static readonly IReadOnlyDictionary<Move, ICubieCube> BasicCubesByMove = GetMoveCubes();

        private static IReadOnlyDictionary<Move, ICubieCube> GetMoveCubes()
        {
            var table = new Dictionary<Move, ICubieCube>();

            foreach (var moveEnum in Extensions.GetEnumValues<Move>())
            {
                var (color, number) = moveEnum.Deconstruct();

                var cc = SolvedCube.Instance.Clone();

                for (var i = 0; i < number; i++) cc.Multiply(BasicCubesByColor[color]);

                table[moveEnum] = new NamedCubieCube(moveEnum.GetDisplayName(), cc);
            }

            return table;
        }

    }
}