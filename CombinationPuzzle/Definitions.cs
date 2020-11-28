
using System.Collections.Generic;

namespace CombinationPuzzle
{
    public static class Definitions {

        public static IReadOnlyList<IReadOnlyList<FaceletPosition>> CornerFacelet = new List<IReadOnlyList<FaceletPosition>> {
            new List<FaceletPosition> {
                FaceletPosition.U9,
                FaceletPosition.R1,
                FaceletPosition.F3
            },
            new List<FaceletPosition> {
                FaceletPosition.U7,
                FaceletPosition.F1,
                FaceletPosition.L3
            },
            new List<FaceletPosition> {
                FaceletPosition.U1,
                FaceletPosition.L1,
                FaceletPosition.B3
            },
            new List<FaceletPosition> {
                FaceletPosition.U3,
                FaceletPosition.B1,
                FaceletPosition.R3
            },
            new List<FaceletPosition> {
                FaceletPosition.D3,
                FaceletPosition.F9,
                FaceletPosition.R7
            },
            new List<FaceletPosition> {
                FaceletPosition.D1,
                FaceletPosition.L9,
                FaceletPosition.F7
            },
            new List<FaceletPosition> {
                FaceletPosition.D7,
                FaceletPosition.B9,
                FaceletPosition.L7
            },
            new List<FaceletPosition> {
                FaceletPosition.D9,
                FaceletPosition.R9,
                FaceletPosition.B7
            }
        };

        public static IReadOnlyList<IReadOnlyList<FaceletPosition>> EdgeFacelet = new List<IReadOnlyList<FaceletPosition>> {
            new List<FaceletPosition> {
                FaceletPosition.U6,
                FaceletPosition.R2
            },
            new List<FaceletPosition> {
                FaceletPosition.U8,
                FaceletPosition.F2
            },
            new List<FaceletPosition> {
                FaceletPosition.U4,
                FaceletPosition.L2
            },
            new List<FaceletPosition> {
                FaceletPosition.U2,
                FaceletPosition.B2
            },
            new List<FaceletPosition> {
                FaceletPosition.D6,
                FaceletPosition.R8
            },
            new List<FaceletPosition> {
                FaceletPosition.D2,
                FaceletPosition.F8
            },
            new List<FaceletPosition> {
                FaceletPosition.D4,
                FaceletPosition.L8
            },
            new List<FaceletPosition> {
                FaceletPosition.D8,
                FaceletPosition.B8
            },
            new List<FaceletPosition> {
                FaceletPosition.F6,
                FaceletPosition.R4
            },
            new List<FaceletPosition> {
                FaceletPosition.F4,
                FaceletPosition.L6
            },
            new List<FaceletPosition> {
                FaceletPosition.B6,
                FaceletPosition.L4
            },
            new List<FaceletPosition> {
                FaceletPosition.B4,
                FaceletPosition.R6
            }
        };

        public static IReadOnlyList<IReadOnlyList<FaceColor>> CornerColor = new List<IReadOnlyList<FaceColor>> {
            new List<FaceColor> {
                FaceColor.U,
                FaceColor.R,
                FaceColor.F
            },
            new List<FaceColor> {
                FaceColor.U,
                FaceColor.F,
                FaceColor.L
            },
            new List<FaceColor> {
                FaceColor.U,
                FaceColor.L,
                FaceColor.B
            },
            new List<FaceColor> {
                FaceColor.U,
                FaceColor.B,
                FaceColor.R
            },
            new List<FaceColor> {
                FaceColor.D,
                FaceColor.F,
                FaceColor.R
            },
            new List<FaceColor> {
                FaceColor.D,
                FaceColor.L,
                FaceColor.F
            },
            new List<FaceColor> {
                FaceColor.D,
                FaceColor.B,
                FaceColor.L
            },
            new List<FaceColor> {
                FaceColor.D,
                FaceColor.R,
                FaceColor.B
            }
        };

        public static IReadOnlyList<IReadOnlyList<FaceColor>> EdgeColor = new List<IReadOnlyList<FaceColor>> {
            new List<FaceColor> {
                FaceColor.U,
                FaceColor.R
            },
            new List<FaceColor> {
                FaceColor.U,
                FaceColor.F
            },
            new List<FaceColor> {
                FaceColor.U,
                FaceColor.L
            },
            new List<FaceColor> {
                FaceColor.U,
                FaceColor.B
            },
            new List<FaceColor> {
                FaceColor.D,
                FaceColor.R
            },
            new List<FaceColor> {
                FaceColor.D,
                FaceColor.F
            },
            new List<FaceColor> {
                FaceColor.D,
                FaceColor.L
            },
            new List<FaceColor> {
                FaceColor.D,
                FaceColor.B
            },
            new List<FaceColor> {
                FaceColor.F,
                FaceColor.R
            },
            new List<FaceColor> {
                FaceColor.F,
                FaceColor.L
            },
            new List<FaceColor> {
                FaceColor.B,
                FaceColor.L
            },
            new List<FaceColor> {
                FaceColor.B,
                FaceColor.R
            }
        };

        public const ushort NPerm4 = 24;

        public const int NChoose84 = 70;

        public const int NMove = 18;

        public const int NTwist = 2187;

        public const int NFlip = 2048;

        public const int NSliceSorted = 11880;

        public const int NSlice = NSliceSorted / NPerm4;

        public const int NFlipsliceClass = 64430;

        public const int NUEdgesPhase2 = 1680;

        public const int NCorners = 40320;

        public const int NCornersClass = 2768;

        public const int NUdEdges = 40320;

        public const int NSym = 48;

        public const int NSymD4H = 16;
    }
}
