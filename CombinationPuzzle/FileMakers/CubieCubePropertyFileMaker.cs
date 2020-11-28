using CombinationPuzzle.BasicCubes;

namespace CombinationPuzzle.FileMakers
{
    public abstract class CubieCubePropertyFileMaker : FileMaker<ushort>
    {
        /// <summary>
        /// The Cube Property to examine.
        /// </summary>
        public abstract CubeProperty CubeProperty { get; }


        /// <inheritdoc />
        public override ushort[] Create()
        {
            var resultTable = new ushort[CubeProperty.MaxI * Definitions.NMove];
            for (ushort i = 0; i < CubeProperty.MaxI; i++)
            {
                var a = SolvedCube.Instance.Clone();
                CubeProperty.SetValue(a, i);

                foreach (var move in Extensions.GetEnumValues<Move>())
                {
                    if (CubeProperty.MovesToIgnore != null && CubeProperty.MovesToIgnore.Contains(move)) continue;
                    var multiplyCube = MoveCubes.BasicCubesByMove[move];
                    var a2 = a.Clone();
                    if(CubeProperty.IsEdges)
                        a2.EdgeMultiply(multiplyCube);
                    else
                        a2.CornerMultiply(multiplyCube);

                    var newValue = CubeProperty.GetValue(a2);
                    resultTable[Definitions.NMove * i + (int) move] = newValue;
                }
            }

            return resultTable;
        }
    }
}