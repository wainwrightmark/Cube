using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using CSharpFunctionalExtensions;

namespace CombinationPuzzle.Cubie
{
#pragma warning disable 660, 661
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public sealed class MutableCubieCube : ICubieCube
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning restore 660, 661
    {
        //TODO make readonly again
        private readonly int[] _cornerOrientations; //TODO byte[] //TODO compress even more
        private readonly Corner[] _cornerPositions; //TODO byte[] //TODO compress even more
        private readonly int[] _edgeOrientations; //TODO byte[] //TODO compress even more
        private readonly Edge[] _edgePositions; //TODO byte[] //TODO compress even more

        public IReadOnlyList<int> CornerOrientations => _cornerOrientations;

        public IReadOnlyList<Corner> CornerPositions => _cornerPositions;

        public IReadOnlyList<int> EdgeOrientations => _edgeOrientations;

        public IReadOnlyList<Edge> EdgePositions => _edgePositions;

        public MutableCubieCube(Corner[] cornerPositions, int[] cornerOrientations, Edge[] edgePositions, int[] edgeOrientations)
        {
            _cornerOrientations = cornerOrientations;
            _cornerPositions = cornerPositions;
            _edgePositions = edgePositions;
            _edgeOrientations = edgeOrientations;
        }

        public MutableCubieCube Clone()
        {
            return new MutableCubieCube(_cornerPositions.ToArray(), _cornerOrientations.ToArray(), _edgePositions.ToArray(), _edgeOrientations.ToArray());
        }

        // Print string for a MoveCubes cube.
        public override string ToString() {
            var sb = new StringBuilder();
            foreach (var i in Extensions.GetEnumValues<Corner>().Cast<int>()) {
                sb.Append($"({CornerPositions[i]},{CornerOrientations[i]})");
            }

            sb.AppendLine();
            foreach (var i in Extensions.GetEnumValues<Edge>().Cast<int>())
            {
                sb.Append($"({EdgePositions[i]},{EdgeOrientations[i]})");
            }
            return sb.ToString();
        }

        #region Equality

        // Define equality of two MoveCubes cubes.
        /// <inheritdoc />
#pragma warning disable 659 //TODO think MoveCubes equality
        public override bool Equals(object? obj)

        {
            return obj is ICubieCube other && Equals(other);
        }

        public static bool operator ==(MutableCubieCube obj1, ICubieCube? obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;

            return obj1.Equals(obj2);
        }

        public static bool operator !=(MutableCubieCube obj1, ICubieCube? obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(ICubieCube other)
        {
            return CubeComparer.Instance.Equals(this, other);
        }
        #pragma warning restore 659
        #endregion



        // Multiply this MoveCubes cube with another MoveCubes cube b, restricted to the corners. Does not change b.
        public void CornerMultiply(ICubieCube b) {
            var cPerm = new Corner[8]; //TODO stackalloc
            var cOri = new int[8];
            var ori = 0;
            foreach (var c in Extensions.GetEnumValues<Corner>().Cast<int>()) {
                cPerm[c] = _cornerPositions[(int) b.CornerPositions[c]];
                var oriA = _cornerOrientations[(int) b.CornerPositions[c]];
                var oriB = b.CornerOrientations[c];
                if (oriA < 3 && oriB < 3) {
                    // two regular cubes
                    ori = oriA + oriB;
                    if (ori >= 3) {
                        ori -= 3;
                    }
                } else if (oriA < 3 && 3 <= oriB) {
                    // cube b is in a mirrored state
                    ori = oriA + oriB;
                    if (ori >= 6) {
                        ori -= 3;
                    }
                } else if (oriA >= 3 && 3 > oriB) {
                    // cube a is in a mirrored state
                    ori = oriA - oriB;
                    if (ori < 3) {
                        ori += 3;
                    }
                } else if (oriA >= 3 && oriB >= 3) {
                    // if both cubes are in mirrored states
                    ori = oriA - oriB;
                    if (ori < 0) {
                        ori += 3;
                    }
                }
                cOri[c] = ori;
            }
            cPerm.CopyTo(_cornerPositions, 0);
            cOri.CopyTo(_cornerOrientations, 0);
        }

        //  Multiply this MoveCubes cube with another cubiecube b, restricted to the Edge. Does not change b. //TODO comments should be summary comments
        public void EdgeMultiply(ICubieCube b) {
            var ePerm = new Edge[12]; //TODO stackalloc
            var eOri = new int[12];
            foreach (var e in Extensions.GetEnumValues<Edge>().Cast<int>()) {
                ePerm[e] = EdgePositions[(int) b.EdgePositions[e]];
                eOri[e] = (b.EdgeOrientations[e] + EdgeOrientations[(int) b.EdgePositions[e]]) % 2;
            }

            ePerm.CopyTo(_edgePositions, 0);
            eOri.CopyTo(_edgeOrientations, 0);
        }

        /// <summary>
        /// Multiply with another MoveCubes cube.
        /// </summary>
        public void Multiply(ICubieCube b) {
            CornerMultiply(b);
            EdgeMultiply(b);
        }

        public void set_twist(int twist) {
            var twistParity = 0;

            for (var i = 6; i >= 0; i--)
            {
                _cornerOrientations[i] = twist % 3;
                twistParity += CornerOrientations[i];
                twist /= 3;
            }

            _cornerOrientations[(int) Corner.Drb] = (3 - twistParity % 3) % 3;
        }


        public void set_flip(int flip) {
            var flipParity = 0;

            for (var i = 10; i >= 0; i--)
            {
                _edgeOrientations[i] = flip % 2;
                flipParity += flip % 2;
                flip /= 2;
            }

            _edgeOrientations[(int) Edge.Br] = (2 - flipParity % 2) % 2;
        }


        public void set_slice(ushort idx)
        {

            var a = Convert.ToInt32(idx);

            var sliceX = 4; //set slice edges
            var otherX = 0;
            foreach (var j in Extensions.GetEnumValues<Edge>().Cast<int>())
            {
                var a1 = a - Miscellaneous.BinomialChoose(11 - j, sliceX);

                if (a1 >= 0)
                {
                    _edgePositions[j] = SliceEdge[4 - sliceX];
                    sliceX -= 1;
                    a = a1;
                }
                else
                {
                    _edgePositions[j] = OtherEdge[otherX];
                    otherX += 1;
                }
            }
        }

        private static readonly ImmutableArray<Edge> SliceEdge = new List<Edge> {
            Edge.Fr,
            Edge.Fl,
            Edge.Bl,
            Edge.Br
        }.ToImmutableArray();
        private static readonly ImmutableArray<Edge> OtherEdge = new List<Edge> {
            Edge.Ur,
            Edge.Uf,
            Edge.Ul,
            Edge.Ub,
            Edge.Dr,
            Edge.Df,
            Edge.Dl,
            Edge.Db
        }.ToImmutableArray();


        public void set_slice_sorted(int idx)
        {
            var b = idx % 24;
            var a = idx / 24;

            var mySliceEdge = SliceEdge.ToArray();

            var j = 1;
            while (j < 4) {
                var k = b % (j + 1);
                b /= j + 1;
                while (k > 0) {
                    mySliceEdge.rotate_right(0, j);
                    k -= 1;
                }
                j += 1;
            }
            var sliceX = 4;
            var otherX = 0;
            foreach (var j2 in Extensions.GetEnumValues<Edge>().Cast<int>())
            {
                if (a - Miscellaneous. BinomialChoose(11 - j2, sliceX) >= 0) {
                    _edgePositions[j2] = mySliceEdge[4 - sliceX];
                    a -= Miscellaneous.BinomialChoose(11 - j2, sliceX);
                    sliceX -= 1;
                }
                else
                {
                    _edgePositions[j2] = OtherEdge[otherX];
                    otherX += 1;
                }
            }
        }

        private static readonly ImmutableArray<Edge> USliceEdge = new List<Edge> {
            Edge.Ur, Edge.Uf, Edge.Ul, Edge.Ub
        }.ToImmutableArray();
        private static readonly ImmutableArray<Edge> UOtherEdge = new List<Edge> {
            Edge.Dr, Edge.Df, Edge.Dl, Edge.Db, Edge.Fr, Edge.Fl, Edge.Bl, Edge.Br
        }.ToImmutableArray();

        public void set_u_edges(int idx)
        {
            var b = idx % 24;
            var a = idx / 24;
            var sliceEdge = USliceEdge.ToArray();

            var j = 1;
            while (j < 4) {
                var k = b % (j + 1);
                b /= j + 1;
                while (k > 0) {
                    sliceEdge.rotate_right(0, j);
                    k -= 1;
                }
                j += 1;
            }

            var sliceX = 4;
            var otherX = 0;
            foreach (var j4 in Extensions.GetEnumValues<Edge>().Cast<int>())
            {
                if (a - Miscellaneous. BinomialChoose(11 - j4, sliceX) >= 0)
                {
                    _edgePositions[j4] = sliceEdge[4 - sliceX];
                    a -= Miscellaneous.BinomialChoose(11 - j4, sliceX);
                    sliceX -= 1;
                }
                else
                {
                    _edgePositions[j4] = UOtherEdge[otherX];
                    otherX += 1;
                }
            }

            for(var j3 = 0; j3 < 4; j3++)
                _edgePositions.rotate_left(0, 11);
        }

        private static readonly ImmutableArray<Edge> DSliceEdge = new List<Edge>
        {
            Edge.Dr, Edge.Df, Edge.Dl, Edge.Db
        }.ToImmutableArray();

        private static readonly ImmutableArray<Edge> DOtherEdge = new List<Edge>
        {
            Edge.Fr, Edge.Fl, Edge.Bl, Edge.Br, Edge.Ur, Edge.Uf, Edge.Ul, Edge.Ub
        }.ToImmutableArray();

        public void set_d_edges(int idx)
        {
            var b = idx % 24;
            var a = idx / 24;

            //Array.Fill(_edgePositions, -1);

            var sliceEdge = DSliceEdge.ToArray();

            var j = 1;
            while (j < 4) {
                var k = b % (j + 1);
                b /= j + 1;
                while (k > 0) {
                    sliceEdge.rotate_right(0, j);
                    k -= 1;
                }
                j += 1;
            }


            var sliceX = 4;
            var otherX = 0;
            foreach (var j2 in Extensions.GetEnumValues<Edge>().Cast<int>()) {
                if (a - Miscellaneous.BinomialChoose(11 - j2, sliceX) >= 0) {
                    _edgePositions[j2] = sliceEdge[4 - sliceX];
                    a -= Miscellaneous.BinomialChoose(11 - j2, sliceX);
                    sliceX -= 1;
                }
                else
                {
                    _edgePositions[j2] = DOtherEdge[otherX];
                    otherX += 1;
                }
            }

            for(var j4 = 0; j4 < 4; j4++) _edgePositions.rotate_left(0, 11);
        }


        public void set_corners(int idx)
        {
            Extensions.GetEnumValues<Corner>().ToArray() .CopyTo(_cornerPositions, 0);

            foreach (var j in Extensions.GetEnumValues<Corner>().Cast<int>()) {
                var k = idx % (j + 1);
                idx /= j + 1;
                while (k > 0) {
                    _cornerPositions.rotate_right(0, j);
                    k -= 1;
                }
            }
        }


        /// <summary>
        /// positions of FR FL BL BR edges are not affected
        /// </summary>
        /// <param name="idx"></param>
        public void set_ud_edges(int idx)
        {
            var ep = Extensions.GetEnumValues<Edge>().ToArray();


            foreach (var j in Extensions.GetEnumValues<Edge>().Take(8).Cast<int>()) {
                var k = idx % (j + 1);
                idx /= j + 1;

                for (var i = 0; i < k; i++) ep.rotate_right(0, j);
            }

            ep.CopyTo(_edgePositions,0);
        }

        // Generate a random cube. The probability is the same for all possible states.
        public void Randomize(Random random) {
            void SetEdges(int idx)
            {
                Extensions.GetEnumValues<Edge>().ToArray().AsSpan().CopyTo(_edgePositions);
                foreach (var j in Extensions.GetEnumValues<Edge>().Cast<int>())
                {
                    var k = idx % (j + 1);
                    idx /= j + 1;
                    while (k > 0)
                    {
                        _edgePositions.rotate_right(0, j);
                        k -= 1;
                    }
                }
            }


            SetEdges(random.Next(479001600));
            var p = this.GetEdgeParity();
            while (true) {
                set_corners(random.Next(40320));
                if (p == this.GetCornerParity()) {
                    // parities of edge and corner permutations must be the same
                    break;
                }
            }
            set_flip(random.Next(2048));
            set_twist(random.Next(2187));
        }

        /// <summary>
        /// Check if this CubieCube is valid
        /// </summary>
        /// <returns></returns>
        public Result Verify()
        {
            var edgeCount = Extensions.GetEnumValues<Edge>().ToDictionary(x=>x, _=>0);
            foreach (var i in Extensions.GetEnumValues<Edge>().Cast<int>()) edgeCount[ EdgePositions[i]] += 1;

            if (Extensions.GetEnumValues<Edge>().Any(i => edgeCount[i] != 1))
                return Result.Failure("Error: Some edges are undefined.");

            var s = Extensions.GetEnumValues<Edge>().Cast<int>().Sum(i => EdgeOrientations[i]);

            if (s % 2 != 0)
                return Result.Failure("Error: Total edge flip is wrong.");
            var cornerCount = new int[8];

            foreach (var i in Extensions.GetEnumValues<Corner>().Cast<int>()) cornerCount[(int) CornerPositions[i]] += 1;

            if (Extensions.GetEnumValues<Corner>().Cast<int>() .Any(i => cornerCount[i] != 1))
                return Result.Failure("Error: Some corners are undefined.");

            s = Extensions.GetEnumValues<Corner>().Cast<int>() .Sum(i => CornerOrientations[i]);
            if (s % 3 != 0)
                return Result.Failure("Error: Total corner twist is wrong.");
            if (this.GetEdgeParity() != this.GetCornerParity())
                return Result.Failure("Error: Wrong edge and corner parity");
            return Result.Success();
        }
    }
}