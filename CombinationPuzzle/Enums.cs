using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CombinationPuzzle
{



    // ""
    //     The names of the facelet positions of the cube
    //                   |************|
    //                   |*U1**U2**U3*|
    //                   |************|
    //                   |*U4**U5**U6*|
    //                   |************|
    //                   |*U7**U8**U9*|
    //                   |************|
    //      |************|************|************|************|
    //      |*L1**L2**L3*|*F1**F2**F3*|*R1**R2**R3*|*B1**B2**B3*|
    //      |************|************|************|************|
    //      |*L4**L5**L6*|*F4**F5**F6*|*R4**R5**R6*|*B4**B5**B6*|
    //      |************|************|************|************|
    //      |*L7**L8**L9*|*F7**F8**F9*|*R7**R8**R9*|*B7**B8**B9*|
    //      |************|************|************|************|
    //                   |************|
    //                   |*D1**D2**D3*|
    //                   |************|
    //                   |*D4**D5**D6*|
    //                   |************|
    //                   |*D7**D8**D9*|
    //                   |************|
    //     A cube definition string "UBL..." means for example: In position U1 we have the U-color, in position U2 we have the
    //     B-color, in position U3 we have the L color etc. according to the order U1, U2, U3, U4, U5, U6, U7, U8, U9, R1, R2,
    //     R3, R4, R5, R6, R7, R8, R9, F1, F2, F3, F4, F5, F6, F7, F8, F9, D1, D2, D3, D4, D5, D6, D7, D8, D9, L1, L2, L3, L4,
    //     L5, L6, L7, L8, L9, B1, B2, B3, B4, B5, B6, B7, B8, B9 of the enum constants.
    //


    /// <summary>
    /// The names of the facelet positions of the cube
    /// </summary>
    public enum FaceletPosition : byte
    {
        U1 = 0,

        U2 = 1,

        U3 = 2,

        U4 = 3,

        U5 = 4,

        U6 = 5,

        U7 = 6,

        U8 = 7,

        U9 = 8,

        R1 = 9,

        R2 = 10,

        R3 = 11,

        R4 = 12,

        R5 = 13,

        R6 = 14,

        R7 = 15,

        R8 = 16,

        R9 = 17,

        F1 = 18,

        F2 = 19,

        F3 = 20,

        F4 = 21,

        F5 = 22,

        F6 = 23,

        F7 = 24,

        F8 = 25,

        F9 = 26,

        D1 = 27,

        D2 = 28,

        D3 = 29,

        D4 = 30,

        D5 = 31,

        D6 = 32,

        D7 = 33,

        D8 = 34,

        D9 = 35,

        L1 = 36,

        L2 = 37,

        L3 = 38,

        L4 = 39,

        L5 = 40,

        L6 = 41,

        L7 = 42,

        L8 = 43,

        L9 = 44,

        B1 = 45,

        B2 = 46,

        B3 = 47,

        B4 = 48,

        B5 = 49,

        B6 = 50,

        B7 = 51,
        B8 = 52,
        B9 = 53,
    }




    //public static class FaceletPosition //TODO change everything to bytes

    //{

    //    public const int U1 = 0;

    //    public const int U2 = 1;

    //    public const int U3 = 2;

    //    public const int U4 = 3;

    //    public const int U5 = 4;

    //    public const int U6 = 5;

    //    public const int U7 = 6;

    //    public const int U8 = 7;

    //    public const int U9 = 8;

    //    public const int R1 = 9;

    //    public const int R2 = 10;

    //    public const int R3 = 11;

    //    public const int R4 = 12;

    //    public const int R5 = 13;

    //    public const int R6 = 14;

    //    public const int R7 = 15;

    //    public const int R8 = 16;

    //    public const int R9 = 17;

    //    public const int F1 = 18;

    //    public const int F2 = 19;

    //    public const int F3 = 20;

    //    public const int F4 = 21;

    //    public const int F5 = 22;

    //    public const int F6 = 23;

    //    public const int F7 = 24;

    //    public const int F8 = 25;

    //    public const int F9 = 26;

    //    public const int D1 = 27;

    //    public const int D2 = 28;

    //    public const int D3 = 29;

    //    public const int D4 = 30;

    //    public const int D5 = 31;

    //    public const int D6 = 32;

    //    public const int D7 = 33;

    //    public const int D8 = 34;

    //    public const int D9 = 35;

    //    public const int L1 = 36;

    //    public const int L2 = 37;

    //    public const int L3 = 38;

    //    public const int L4 = 39;

    //    public const int L5 = 40;

    //    public const int L6 = 41;

    //    public const int L7 = 42;

    //    public const int L8 = 43;

    //    public const int L9 = 44;

    //    public const int B1 = 45;

    //    public const int B2 = 46;

    //    public const int B3 = 47;

    //    public const int B4 = 48;

    //    public const int B5 = 49;

    //    public const int B6 = 50;

    //    public const int B7 = 51;

    //    public const int B8 = 52;

    //    public const int B9 = 53;

    //    /// <summary>
    //    /// 54 values
    //    /// </summary>
    //    public static readonly ImmutableArray<int> Values = Enumerable.Range(0, 54).ToImmutableArray();
    //}

    /// <summary>
    /// The possible colors of the cube facelets. Colors U refers to the color of the U(p)-Face etc.
    /// Also used to name the faces itself.
    /// </summary>
    public enum FaceColor : byte
    {
        [Display(Name= "Yellow")]
        U = 0,
        [Display(Name= "Green")]
        R = 1,
        [Display(Name= "Red")]
        F = 2,
        [Display(Name= "White")]
        D = 3,
        [Display(Name= "Blue")]
        L = 4,
        [Display(Name= "Orange")]
        B = 5
    }

    public enum HorizontalPositionEnum
    {
        Left = 0,
        Middle =1,
        Right = 2
    }

    public enum VerticalPositionEnum
    {
        Top = 0,
        Middle = 1,
        Bottom = 2
    }

    public static class PositionExtensions
    {
        public static int GetNumber(this (HorizontalPositionEnum h, VerticalPositionEnum p) pair) => ((int) pair.p * 3) + (int) pair.h;
    }

    /// <summary>
    /// The names of the corner positions of the cube. Corner URF e.g. has an U(p), a R(ight) and a FaceletPosition(ront) facelet.
    /// </summary>
    public enum Corner
    {
        Urf = 0,
        Ufl = 1,
        Ulb = 2,
        Ubr = 3,
        Dfr = 4,
        Dlf = 5,
        Dbl = 6,
        Drb = 7
    }

    /// <summary>
    /// The names of the edge positions of the cube. Edges UR e.g. has an U(p) and R(ight) facelet.
    /// </summary>
    public enum Edge
    {
        Ur = 0,
        Uf = 1,
        Ul = 2,
        Ub = 3,
        Dr = 4,
        Df = 5,
        Dl = 6,
        Db = 7,
        Fr = 8,
        Fl = 9,
        Bl = 10,
        Br = 11
    }

    /// <summary>
    /// The MoveExtensions in the faceTurn metric.
    /// </summary>
    public enum Move : byte  { U1 = 0, U2 = 1, U3 = 2, R1 = 3, R2 = 4, R3 = 5, F1 = 6, F2 = 7, F3 = 8, D1 = 9, D2 = 10, D3 = 11, L1 = 12, L2 = 13, L3 = 14, B1 = 15, B2 = 16, B3 = 17, }

    /// <summary>
    /// The MoveExtensions in the faceTurn metric.Not to be confused with the names of the facelet positions in class FaceletPosition.
    /// </summary>
    public static class MoveExtensions
    {
        public static string GetDisplayName(this Move move, Orientation orientation, bool inverse)
        {

            var (color, number) = move.Deconstruct();

            var newColor = inverse? orientation.GetInversePosition(color) : orientation.GetNewPosition(color);

            string suffix = number switch
            {
                1 => "",
                2 => "2",
                3 => "`",
                _ => throw new ArgumentException($"{move} not recognized")
            };

            return newColor + suffix;
        }

        /// <summary>
        /// Deconstructs the move into the color and the number of applications (1, 2, or 3)
        /// </summary>
        public static (FaceColor color, int number) Deconstruct(this Move move)
        {
            var color = (FaceColor) ((int) move / 3);
            var number = ((int) move % 3) + 1;

            return (color, number);
        }

        public static Move ToMove(this (FaceColor color, int number) pair)
        {
            var (color, number) = pair;
            var i = (int) color * 3 + number - 1;

            return (Move) i;
        }

        /// <summary>
        /// Are both moves on the same face.
        /// e.g. U1 then U2
        /// </summary>
        public static bool IsSameFace(this Move move1, Move move2) => (int) move1 / 3 == (int) move2 / 3;

        /// <summary>
        /// Can one move precede the other in a solution.
        /// Two moves on the same face can't follow each other.
        /// Two moves on the same axis can't follow each other if they are in reverse order.
        /// e.g. U1 then D1 is fine but D1 then U1 is not.
        /// </summary>
        public static bool CanPrecede(this Move move1, Move move2)
        {
            var face1 = (int) move1 / 3;
            var face2 = (int) move2 / 3;

            return face1 != face2 && face1 != face2 + 3;
        }


        public static readonly ImmutableArray<Move> AllMoves = Extensions.GetEnumValues<Move>().ToImmutableArray();

        public static readonly ImmutableArray<ImmutableArray<Move>> PossibleNextMovesPhase1 =
            AllMoves.Select(previous => AllMoves.Where(m2 => previous.CanPrecede(m2)).ToImmutableArray())
                .ToImmutableArray();

        public static readonly ImmutableArray<Move> Phase2MoveEnums = new[] {Move.U1, Move.U2, Move.U3, Move.R2, Move.F2, Move.D1, Move.D2, Move.D3, Move.L2, Move.B2}.ToImmutableArray();

        public static readonly ImmutableArray<ImmutableArray<Move>> PossibleNextMovesPhase2 =
            AllMoves.Select(previous => Phase2MoveEnums.Where(m2 => previous.CanPrecede(m2)).ToImmutableArray())
                .ToImmutableArray();

        public static Move Inverse(this Move move)
        {
            var m = ((int)move / 3) * 3;
            var n = 2 - (int) move % 3;

            var r = m + n;
            return (Move) r;
        }



    }

    /// <summary>
    /// Basic symmetries of the cube. All 48 cube symmetries can be generated by sequences of these 4 symmetries.
    /// </summary>
    public enum BasicSymmetry : byte
    {
        RotateUrf3 = 0,
        RotateF2 = 1,
        RotateU4 = 2,
        MirrorLr2 = 3
    }
}
