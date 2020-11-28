using System;

namespace CombinationPuzzle
{
    public static class Pruning
    {

        public static sbyte GetDistance(int maxPhase1Length, uint flipsliceTwistDepthMod3)
        {
            var index = 3 * maxPhase1Length + flipsliceTwistDepthMod3;

            var r = Distance[(int) index];

            return r;
        }

        /// <summary>
        ///array distance computes the new distance from the old_distance i and the new_distance_mod3 j.
        ///We need this array because the pruning tables only store the distances mod 3.
        /// </summary>
        private static sbyte[] Distance = CreateDistanceTable();

        private static sbyte[] CreateDistanceTable()
        {
            var distanceArray = new sbyte[60];
            for (var i = 0; i < 20; i++)
            for (var j = 0; j < 3; j++)
            {
                var d = (i / 3) * 3 + j;
                switch (i % 3)
                {
                    case 2 when j == 0:
                        d += 3;
                        break;
                    case 0 when j == 2:
                        d -= 3;
                        break;
                }

                distanceArray[3 * i + j] = Convert.ToSByte(d);
            }

            return distanceArray;
        }

    }
}