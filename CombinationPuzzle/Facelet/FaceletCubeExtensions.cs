using System;
using System.Linq;
using System.Text;
using CombinationPuzzle.Cubie;
using CSharpFunctionalExtensions;

namespace CombinationPuzzle.Facelet
{
    public static class FaceletCubeExtensions
    {
        public static Result ValidateColors(this IFaceletCube fc)
        {
            var counts = Extensions.GetEnumValues<FaceColor>().ToDictionary(x=>x,_=> 0);
            var countUnknown = 0;

            foreach (var facelet in Extensions.GetEnumValues<FaceletPosition>())
            {
                var c = fc[(int) facelet];
                if (c.HasValue)
                    counts[c!.Value]++;
                else countUnknown++;
            }

            var result = counts.Select(kvp=> Result.SuccessIf(kvp.Value == 9, $"{kvp.Value} {kvp.Key.GetDisplayName()}"))

                .Combine()
                .Bind(()=>Result.SuccessIf(countUnknown == 0, $"{countUnknown} unknown cells"));

            return result;
        }

        /// <summary>
        /// Give a string representation of the facelet cube.
        /// </summary>
        /// <param name="faceletCube"></param>
        /// <returns></returns>
        public static string Serialize(this IFaceletCube faceletCube)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 0 + 54; i++)
                sb.Append(faceletCube[i]?.ToString()??"?");

            return sb.ToString();
        }

        /// <summary>
        /// Give a 2dstring representation of a facelet cube.
        /// </summary>
        /// <param name="faceletCube"></param>
        /// <returns></returns>
        public static string To2dString(this IFaceletCube faceletCube)
        {
            var s = faceletCube.Serialize();
            var r = $"   {s[0..3]}\n   {s[3..6]}\n   {s[6..9]}\n";
            r +=
                $"{s[36..39]}{s[18..21]}{s[9..12]}{s[45..48]}\n{s[39..42]}{s[21..24]}{s[12..15]}{s[48..51]}\n{s[42..45]}{s[24..27]}{s[15..18]}{s[51..54]}\n";
            r += $"   {s[27..30]}\n   {s[30..33]}\n   {s[33..36]}\n";
            return r;
        }

        /// <summary>
        /// Return a MoveCubes representation of the facelet cube.
        /// </summary>
        /// <param name="faceletCube"></param>
        /// <returns></returns>
        public static Result<MutableCubieCube> ToCubieCube(this IFaceletCube faceletCube)
        {
            var vResult = faceletCube.ValidateColors();

            if (!vResult.IsSuccess)
                return vResult.ConvertFailure<MutableCubieCube>();

            var cornerPositions = new Corner[8];
            var cornerOrientations = new int[8];
            var edgePositions = new Edge[12];
            var edgeOrientations = new int[12];


            foreach (var i in Extensions.GetEnumValues<Corner>().Cast<int>())
            {
                var fac = Definitions.CornerFacelet[i];

                var cornerOrientation = Enumerable.Range(0, 3)
                    .FirstOrDefault(o =>
                        faceletCube[(int)fac[o]] == FaceColor.U || faceletCube[(int)fac[o]] == FaceColor.D);
                var col1 = faceletCube[(int)fac[(cornerOrientation + 1) % 3]];
                var col2 = faceletCube[(int)fac[(cornerOrientation + 2) % 3]];
                foreach (var j in Extensions.GetEnumValues<Corner>())
                {
                    var col = Definitions.CornerColor[(int) j];
                    if (col1 == col[1] && col2 == col[2])
                    {
                        cornerPositions[i] = j;
                        cornerOrientations[i] = cornerOrientation;
                        break;
                    }
                }
            }

            foreach (var i in Extensions.GetEnumValues<Edge>().Cast<int>())
            {
                foreach (var j in Extensions.GetEnumValues<Edge>().Cast<int>())
                {
                    if (faceletCube[(int) Definitions.EdgeFacelet[i][0]] == Definitions.EdgeColor[j][0] &&
                        faceletCube[ (int)Definitions.EdgeFacelet[i][1]] == Definitions.EdgeColor[j][1])
                    {
                        edgePositions[i] = (Edge) j;
                        edgeOrientations[i] = 0;
                        break;
                    }

                    if (faceletCube[(int)Definitions.EdgeFacelet[i][0]] == Definitions.EdgeColor[j][1] &&
                        faceletCube[(int)Definitions.EdgeFacelet[i][1]] == Definitions.EdgeColor[j][0])
                    {
                        edgePositions[i] = (Edge) j;
                        edgeOrientations[i] = 1;
                        break;
                    }
                }
            }

            return new MutableCubieCube(cornerPositions, cornerOrientations, edgePositions, edgeOrientations);
        }

        public static MutableFaceletCube Clone(this IFaceletCube faceletCube)
        {
            return new MutableFaceletCube(faceletCube.Facelets);
        }

        public const string EmptyCubeString = "Empty";
        public const string SolvedCubeString = "Solved";

        /// <summary>
        /// Construct a facelet cube from a string.
        /// </summary>
        public static Result<ImmutableFaceletCube> CreateImmutableFromString(string s)
        {
            if (s.Equals(SolvedCubeString, StringComparison.OrdinalIgnoreCase)) return ImmutableFaceletCube.SolvedCube;


            if (s.Length < 54)
                return Result.Failure<ImmutableFaceletCube>("Error: Cube definition string " + s + " contains fewer than 54 facelets.");
            if (s.Length > 54)
                return Result.Failure<ImmutableFaceletCube>("Error: Cube definition string " + s + " contains more than 54 facelets.");

            var facelets = new FaceColor[54];
            var counts = Extensions.GetEnumValues<FaceColor>().ToDictionary(x => x, _ => 0);

            for (var i = 0; i < 54; i++)
            {
                if (Enum.TryParse<FaceColor>(s[i].ToString(), true, out var color))
                {
                    facelets[i] = color;
                    counts[color]++;
                }
                else
                {
                    return Result.Failure<ImmutableFaceletCube>($"Color '{s[i]}' not recognized.");
                }
            }


            if (counts.Any(x => x.Value != 9))
                return Result.Failure<ImmutableFaceletCube>("Error: Cube definition string " + s +
                                                            " does not contain exactly 9 facelets of each color.");
            var fc = new ImmutableFaceletCube(facelets);

            return fc;
        }

        public static MutableFaceletCube CreateMutableFromString(string s)
        {
            if (s.Equals(SolvedCubeString, StringComparison.OrdinalIgnoreCase)) return MutableFaceletCube.SolvedCube;
            if (s.Equals(EmptyCubeString, StringComparison.OrdinalIgnoreCase)) return MutableFaceletCube.Empty;


            s = s.PadRight(54, '?');
            var facelets = new FaceColor?[54];

            for (var i = 0; i < 0 + 54; i++)
                facelets[i] = Enum.TryParse<FaceColor>(s[i].ToString(),true, out var c)? c : null as FaceColor?;

            var fc = new MutableFaceletCube(facelets);

            return fc;
        }

    }
}