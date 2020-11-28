namespace CombinationPuzzle.Symmetries
{
    /// <summary>
    /// The group table for the 48 symmetries
    /// </summary>
    public static class MultipleSymmetries
    {
        private static int[,] Generate()
        {
            var table = new int[Definitions.NSym, Definitions.NSym];
            for (var i = 0; i < 0 + Definitions.NSym; i++)
            {
                for (var j = 0; j < 0 + Definitions.NSym; j++)
                {
                    var cc = Basic.Cubes[i].Clone();
                    cc.Multiply(Basic.Cubes[j]);
                    for (var k = 0; k < 0 + Definitions.NSym; k++)
                    {
                        if (cc.Equals(Basic.Cubes[k]))
                        {
                            // Basic[i]*Basic[j] == Basic[k]
                            table[i, j] = k;
                            break;
                        }
                    }
                }
            }

            return table;
        }

        public static readonly int[,] MultSym = Generate();
    }
}