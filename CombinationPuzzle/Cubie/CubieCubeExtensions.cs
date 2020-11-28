using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CombinationPuzzle.Coordinate;
using CombinationPuzzle.Data;
using CombinationPuzzle.Facelet;
using CombinationPuzzle.Symmetries;

namespace CombinationPuzzle.Cubie
{
    public static class CubieCubeExtensions
    {
        public static ImmutableFaceletCube ToFaceletCube(this ICubieCube cubieCube)
        {
            var facelets = new FaceColor[Extensions.GetEnumValues<FaceletPosition>().Count()];

            foreach (var i in Extensions.GetEnumValues<Corner>().Cast<int>())
            {
                var corner = cubieCube.CornerPositions[i];
                var orientation = cubieCube.CornerOrientations[i];
                for (var k = 0; k < 3; k++)
                    facelets[(int) Definitions.CornerFacelet[i][(k + orientation) % 3]] = Definitions.CornerColor[(int) corner][k];
            }

            foreach (var i in Extensions.GetEnumValues<Edge>().Cast<int>())
            {
                var edge = cubieCube.EdgePositions[i];
                var orientation = cubieCube.EdgeOrientations[i];
                for (var k = 0; k < 2; k++)
                    facelets[(int) Definitions.EdgeFacelet[i][(k + orientation) % 2]] = Definitions.EdgeColor[(int) edge][k];
            }

            //set centers
            foreach (var c in Extensions.GetEnumValues<FaceColor>())
            {
                var centre = (int)c * 9 + 4;
                facelets[centre] = c;
            }

            var fc = new ImmutableFaceletCube(facelets);

            return fc;
        }

        public static ImmutableCoordinateCube ToCoordinateCube(this ICubieCube cubieCube, DataSource dataSource)
        {
            return new ImmutableCoordinateCube(
                cubieCube.get_flip(),
                cubieCube.get_twist(),
                cubieCube.get_slice_sorted(),
                cubieCube.GetCorners(),
                cubieCube.get_u_edges(),
                cubieCube.get_d_edges(), dataSource);
        }


        /// <summary>
        /// Inverts this cube
        /// </summary>
        /// <returns></returns>
        public static MutableCubieCube Invert(this ICubieCube cubieCube)
        {
            var edgePositions = new Edge[12];
            var edgeOrientations = new int[12];
            var cornerPositions = new Corner[8];
            var cornerOrientations = new int[8];


            foreach (var e in Extensions.GetEnumValues<Edge>().Cast<int>()) edgePositions[(int) cubieCube.EdgePositions[e]] = (Edge) e;

            foreach (var e in Extensions.GetEnumValues<Edge>().Cast<int>()) edgeOrientations[e] = cubieCube.EdgeOrientations[(int) edgePositions[e]];

            foreach (var c in Extensions.GetEnumValues<Corner>()) cornerPositions[(int) cubieCube.CornerPositions[(int) c]] = c;

            foreach (var c in Extensions.GetEnumValues<Corner>().Cast<int>())
            {
                var ori = cubieCube. CornerOrientations[(int) cornerPositions[c]];
                if (ori >= 3)
                    cornerOrientations[c] = ori;
                else
                {
                    cornerOrientations[c] = -ori;
                    if (cornerOrientations[c] < 0) cornerOrientations[c] += 3; //TODO look at this - can orientation be negative?
                }
            }

            return new MutableCubieCube(cornerPositions, cornerOrientations, edgePositions, edgeOrientations);
        }

        /// <summary>
        /// Gets the corner permutation parity. A solvable cube has the same corner and edge parity.
        /// </summary>
        public static int GetCornerParity(this ICubieCube cubieCube)
        {
            var s = 0;

            for (var i = 7; i > 0; i--)
            for (var j = i - 1; j >= 0; j--)
                if (cubieCube.CornerPositions[j] > cubieCube.CornerPositions[i])
                    s += 1;

            return s % 2;
        }

        /// <summary>
        /// Gets the edge permutation parity. A solvable cube has the same corner and edge parity.
        /// </summary>
        /// <param name="cubieCube"></param>
        /// <returns></returns>
        public static int GetEdgeParity(this ICubieCube cubieCube)
        {
            var s = 0;

            for (var i = 11; i > 0 ; i--)
            for (var j = i - 1; j >= 0; j--)
                if (cubieCube.EdgePositions[j] > cubieCube.EdgePositions[i])
                    s += 1;

            return s % 2;
        }


        /// <summary>
        /// Get the symmetries and antisymmetries of the cube
        /// </summary>
        public static IEnumerable<int> Symmetries(this ICubieCube cubieCube)
        {
            for (var j = 0; j < Definitions.NSym; j++)
            {
                var c = Basic.Cubes[j].Clone();
                c.Multiply(cubieCube);
                c.Multiply(Inverse.GetCube(j));

                if (CubeComparer.Instance.Equals(cubieCube, c))
                    yield return j;
                var d = c.Invert();

                if (CubeComparer.Instance.Equals(cubieCube, d))
                    yield return j + Definitions.NSym;
            }
        }

        /// <summary>
        /// Get the twist of the 8 corners.
        /// In phase 1 the twist is between zero and 2186 inclusive.
        /// In phase 2, the twist is zero.
        /// </summary>
        public static ushort get_twist(this ICubieCube cubieCube)
        {
            var ret = 0;
            for (var i = 0; i < 7; i++)
                ret = 3 * ret + cubieCube.CornerOrientations[i];
            var r = Convert.ToUInt16(ret);

            Debug.Assert(r <= 2186);

            return r;
        }

        /// <summary>
        /// Get the flip of the 12 Edge.
        /// The flip is between 0 and 2047 inclusive in phase 1.
        /// The flip is zero in phase 2.
        /// </summary>
        /// <param name="cubieCube"></param>
        /// <returns></returns>
        public static ushort get_flip(this ICubieCube cubieCube)
        {
            var ret = 0;
            for (var i = 0; i < 11; i++) ret = 2 * ret + cubieCube.EdgeOrientations[i];

            Debug.Assert(ret <= 2047);

            return Convert.ToUInt16(ret);
        }

        /// <summary>
        /// Get the location of the UD-slice edges FR,FL,BL and BR ignoring their permutation.
        /// Slice is between 0 and 494 inclusive in phase 1.
        /// Slice is zero in phase 2.
        /// </summary>
        public static ushort get_slice(this ICubieCube cubieCube)
        {
            var a = 0;
            var x = 0;
            // Compute the index a < (12 choose 4)

            for (var j = 11; j >=0; j--)
            {
                if (Edge.Fr > cubieCube.EdgePositions[j] || cubieCube.EdgePositions[j] > Edge.Br) continue;
                a += Miscellaneous.BinomialChoose(11 - j, x + 1);
                x += 1;
            }

            Debug.Assert(a <= 495);

            return Convert.ToUInt16(a);
        }


        /// <summary>
        /// Get the permutation and location of the UD-slice edges FR,FL,BL and BR.
        /// Between 0 and 11879 inclusive in phase 1.
        /// Between 0 and 24 in phase 2.
        /// 0 for solved cube.
        /// </summary>
        /// <param name="cubieCube"></param>
        /// <returns></returns>
        public static ushort get_slice_sorted(this ICubieCube cubieCube)
        {
            var a = 0;
            var x = 0;
            var edge4 = new Edge[4];
            // First compute the index a < (12 choose 4) and the permutation array perm.

            for (var j = 11; j >= 0; j--)
            {
                var edge = cubieCube.EdgePositions[j];


                if (Edge.Fr <= edge && edge <= Edge.Br)
                {
                    a += Miscellaneous.BinomialChoose(11 - j, x + 1);
                    edge4[3 - x] = edge;
                    x += 1;
                }
            }

            // Then compute the index b < 4! for the permutation in edge4
            var b = 0;
            for (var j = 3; j > 0 ; j--)
            {
                var k = 0;
                while ((int) edge4[j] != j + 8)
                {
                    edge4.rotate_left(0, j);
                    k += 1;
                }
                b = (j + 1) * b + k;
            }
            var r = 24 * a + b;
            return Convert.ToUInt16(r);
        }

        private static ushort GetEdges(this ICubieCube cubieCube, int offset)
        {
            var a = 0;
            var x = 0;
            var edge4 = new int[4];
            var epMod = cubieCube.EdgePositions.ToArray();
            for (var k = 0; k < 4; k++)
                epMod.rotate_right(0, 11);

            // First compute the index a < (12 choose 4) and the permutation array perm.
            for (var j = 11; j >= 0; j--)
            {
                var epModJ = (int) epMod[j];
                if (offset <= epModJ && epModJ <= 3 + offset)
                {
                    a += Miscellaneous.BinomialChoose(11 - j, x + 1);
                    edge4[3 - x] = epModJ;
                    x += 1;
                }
            }

            // Then compute the index b < 4! for the permutation in edge4
            var b = 0;
            for (var j = 3; j > 0; j--)
            {
                var k = 0;
                while (edge4[j] != j + offset)
                {
                    edge4.rotate_left(0, j);
                    k += 1;
                }
                b = (j + 1) * b + k;
            }

            return Convert.ToUInt16(24 * a + b) ;
        }

        /// <summary>
        /// Get the permutation and location of edges UR, UF, UL and UB.
        /// Between 0 and 11879 inclusive in phase 1
        /// Between 0 and 1679 inclusive in phase 2
        /// 1656 for solved cube
        /// </summary>
        public static ushort get_u_edges(this ICubieCube cubieCube) => GetEdges(cubieCube, 0);

        /// <summary>
        /// Get the permutation and location of the edges DR, DF, DL and DB.
        /// Between 0 and 11879 inclusive in phase 1
        /// Between 0 and 1679 inclusive in phase 2
        /// 1656 for solved cube
        /// </summary>
        public static ushort get_d_edges(this ICubieCube cubieCube) => GetEdges(cubieCube, 4);


        /// <summary>
        /// Get the permutation of the 8 U and D Edge.
        /// Undefined in phase 1.
        /// Between 0 and  40319 inclusive in phase 2.
        /// 0 in solved cube.
        /// </summary>
        /// <param name="cubieCube"></param>
        /// <returns></returns>
        public static ushort get_ud_edges(this ICubieCube cubieCube)
        {
            var perm = cubieCube.EdgePositions.Take(8).ToArray(); //use slice
            var b = 0;

            for (var j = 7; j > 0; j--)
            {
                var k = 0;
                while ((int) perm[j] != j)
                {
                    perm.rotate_left(0, j);
                    k += 1;
                }
                b = (j + 1) * b + k;
            }
            return Convert.ToUInt16(b);
        }


        /// <summary>
        /// Get the permutation of the 8 corners.
        /// Between 0 and 40319
        /// 0 for solved cube
        /// </summary>
        public static ushort GetCorners(this ICubieCube cubieCube)
        {
            var perm = cubieCube.CornerPositions.ToArray();
            var b = 0;

            for (var j = 7; j > 0; j--)
            {
                var k = 0;
                while (perm[j] != (Corner) j)
                {
                    perm.rotate_left(0, j);
                    k += 1;
                }
                b = (j + 1) * b + k;
            }
            return Convert.ToUInt16(b);
        }


    }
}