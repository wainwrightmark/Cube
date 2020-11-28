using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CombinationPuzzle
{

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public enum RotationType
    {
        None,
        Clockwise,
        Double,
        Anticlockwise,
    }


    public class Orientation
    {
        public static Orientation Default { get; } = new Orientation(FaceColor.U, FaceColor.R, FaceColor.F, Direction.North, Direction.North, Direction.North, Direction.North, Direction.North, Direction.North);


        private ReadonlyTwoWayDictionary<FaceColor, FaceColor> Colors { get; }

        private IReadOnlyDictionary<FaceColor, Direction> Directions { get; }

        private Orientation(FaceColor up, FaceColor right, FaceColor front, Direction upDirection, Direction rightDirection, Direction frontDirection, Direction downDirection, Direction leftDirection, Direction backDirection)
        {
            Directions = new Dictionary<FaceColor, Direction>
            {
                {FaceColor.U, upDirection},
                {FaceColor.R, rightDirection},
                {FaceColor.F, frontDirection},
                {FaceColor.D, downDirection},
                {FaceColor.L, leftDirection},
                {FaceColor.B, backDirection},
            };

            Colors = new ReadonlyTwoWayDictionary<FaceColor, FaceColor>(
                (FaceColor.U, up),
                (FaceColor.R, right),
                (FaceColor.F, front),
                (FaceColor.D, GetOpposite(up)),
                (FaceColor.L, GetOpposite(right)),
                (FaceColor.B, GetOpposite(front))
            );
        }

        public Orientation RotateLeft() => Transform(
            FaceColor.U, FaceColor.B, FaceColor.R,
            RotationType.Clockwise,RotationType.None, RotationType.None,
            RotationType.Anticlockwise, RotationType.None, RotationType.None);


        public Orientation RotateRight() => Transform(
            FaceColor.U, FaceColor.F, FaceColor.L,
            RotationType.Anticlockwise, RotationType.None, RotationType.None,
            RotationType.Clockwise, RotationType.None, RotationType.None);

        public Orientation RotateUp() => Transform(FaceColor.F, FaceColor.R, FaceColor.D,
            RotationType.None, RotationType.Clockwise, RotationType.None,
            RotationType.Double, RotationType.Anticlockwise, RotationType.Double);

        public Orientation RotateDown() => Transform(FaceColor.B, FaceColor.R, FaceColor.U,
            RotationType.Double, RotationType.Anticlockwise, RotationType.None,
            RotationType.None, RotationType.Clockwise, RotationType.Double);

        public Orientation RotateClockwise() => Transform(FaceColor.L, FaceColor.U, FaceColor.F,
            RotationType.Clockwise, RotationType.Clockwise, RotationType.Clockwise,
            RotationType.Clockwise, RotationType.Clockwise, RotationType.Anticlockwise);

        public Orientation RotateAntiClockwise() => Transform(FaceColor.R, FaceColor.D, FaceColor.F,
            RotationType.Anticlockwise, RotationType.Anticlockwise, RotationType.Anticlockwise,
            RotationType.Anticlockwise, RotationType.Anticlockwise, RotationType.Clockwise);

        private Orientation Transform(FaceColor newUp,
            FaceColor newRight,
            FaceColor newFront,
            RotationType upRotationType,
            RotationType rightRotationType,
            RotationType frontRotationType,
            RotationType downRotationType,
            RotationType leftRotationType,
            RotationType backRotationType)
        {
            var up = Colors.Forwards[newUp];
            var right = Colors.Forwards[newRight];
            var front = Colors.Forwards[newFront];


            return new Orientation(up, right, front,
                Move(Directions[newUp], upRotationType),
                Move(Directions[newRight], rightRotationType),
                Move(Directions[newFront], frontRotationType),
                Move(Directions[GetOpposite(newUp)], downRotationType),
                Move(Directions[GetOpposite(newRight)], leftRotationType),
                Move(Directions[GetOpposite(newFront)], backRotationType)
            );
        }

        private static Direction Move(Direction direction, RotationType rotation)
        {
            return rotation switch
            {
                RotationType.None => direction,

                RotationType.Clockwise => direction switch
                {
                    Direction.North => Direction.East,
                    Direction.East => Direction.South,
                    Direction.South => Direction.West,
                    Direction.West => Direction.North,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                },
                RotationType.Double => direction switch
                {
                    Direction.North => Direction.South,
                    Direction.East => Direction.West,
                    Direction.South => Direction.North,
                    Direction.West => Direction.East,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                },
                RotationType.Anticlockwise => direction switch
                {
                    Direction.North => Direction.West,
                    Direction.East => Direction.North,
                    Direction.South => Direction.East,
                    Direction.West => Direction.South,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                },

                _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
            };
        }


        public static IEnumerable<(string name, Func<Orientation, Orientation> function)> AllMoves
        {
            get
            {
                yield return ("↩️", o => o.RotateLeft());
                yield return ("↪️", o => o.RotateRight());
                yield return ("⤴️", o => o.RotateUp());
                yield return ("⤵️", o => o.RotateDown());
                yield return ("🔃", o => o.RotateClockwise());
                yield return ("🔄", o => o.RotateAntiClockwise());
            }
        }

        public IReadOnlyList<IReadOnlyList<(HorizontalPositionEnum hp, VerticalPositionEnum vp)>> GetRows(FaceColor face) => FaceletsDictionary[Directions[Colors.Backwards[face]]];//TODO check this


        private static readonly
            IReadOnlyDictionary<Direction, ImmutableList<IReadOnlyList<(HorizontalPositionEnum hp, VerticalPositionEnum vp)>>>
            FaceletsDictionary =
                Extensions.GetEnumValues<Direction>().ToDictionary(x => x, x => GetFacelets(x).ToImmutableList());

        private static IEnumerable<IReadOnlyList<(HorizontalPositionEnum hp, VerticalPositionEnum vp)>> GetFacelets(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    yield return new[]{(HorizontalPositionEnum.Left, VerticalPositionEnum.Top),(HorizontalPositionEnum.Middle, VerticalPositionEnum.Top),(HorizontalPositionEnum.Right, VerticalPositionEnum.Top),   };
                    yield return new[]{(HorizontalPositionEnum.Left, VerticalPositionEnum.Middle),(HorizontalPositionEnum.Middle, VerticalPositionEnum.Middle),(HorizontalPositionEnum.Right, VerticalPositionEnum.Middle),   };
                    yield return new[]{(HorizontalPositionEnum.Left, VerticalPositionEnum.Bottom),(HorizontalPositionEnum.Middle, VerticalPositionEnum.Bottom),(HorizontalPositionEnum.Right, VerticalPositionEnum.Bottom),   };
                    break;
                case Direction.East:
                    yield return new[] { (HorizontalPositionEnum.Left, VerticalPositionEnum.Bottom), (HorizontalPositionEnum.Left, VerticalPositionEnum.Middle), (HorizontalPositionEnum.Left, VerticalPositionEnum.Top), };
                    yield return new[] { (HorizontalPositionEnum.Middle, VerticalPositionEnum.Bottom), (HorizontalPositionEnum.Middle, VerticalPositionEnum.Middle), (HorizontalPositionEnum.Middle, VerticalPositionEnum.Top), };
                    yield return new[] { (HorizontalPositionEnum.Right, VerticalPositionEnum.Bottom), (HorizontalPositionEnum.Right, VerticalPositionEnum.Middle), (HorizontalPositionEnum.Right, VerticalPositionEnum.Top), };
                    break;

                case Direction.South:
                    yield return new[] { (HorizontalPositionEnum.Right, VerticalPositionEnum.Bottom), (HorizontalPositionEnum.Middle, VerticalPositionEnum.Bottom), (HorizontalPositionEnum.Left, VerticalPositionEnum.Bottom), };
                    yield return new[] { (HorizontalPositionEnum.Right, VerticalPositionEnum.Middle), (HorizontalPositionEnum.Middle, VerticalPositionEnum.Middle), (HorizontalPositionEnum.Left, VerticalPositionEnum.Middle), };
                    yield return new[] { (HorizontalPositionEnum.Right, VerticalPositionEnum.Top), (HorizontalPositionEnum.Middle, VerticalPositionEnum.Top), (HorizontalPositionEnum.Left, VerticalPositionEnum.Top), };
                    break;
                case Direction.West:
                    yield return new[] { (HorizontalPositionEnum.Right, VerticalPositionEnum.Top), (HorizontalPositionEnum.Right, VerticalPositionEnum.Middle), (HorizontalPositionEnum.Right, VerticalPositionEnum.Bottom), };
                    yield return new[] { (HorizontalPositionEnum.Middle, VerticalPositionEnum.Top), (HorizontalPositionEnum.Middle, VerticalPositionEnum.Middle), (HorizontalPositionEnum.Middle, VerticalPositionEnum.Bottom), };
                    yield return new[] { (HorizontalPositionEnum.Left, VerticalPositionEnum.Top), (HorizontalPositionEnum.Left, VerticalPositionEnum.Middle), (HorizontalPositionEnum.Left, VerticalPositionEnum.Bottom), };
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        /// <summary>
        /// Maps the position in space to the color.
        /// </summary>
        /// <param name="thisFace"></param>
        /// <returns></returns>
        public FaceColor GetNewPosition(FaceColor thisFace) => Colors.Forwards[thisFace];

        /// <summary>
        /// Gets the position in space from the color
        /// </summary>
        /// <param name="thisFace"></param>
        /// <returns></returns>
        public FaceColor GetInversePosition(FaceColor thisFace) => Colors.Backwards[thisFace];


        private static FaceColor GetOpposite(FaceColor ce)
        {
            return ce switch
            {
                FaceColor.U => FaceColor.D,
                FaceColor.R => FaceColor.L,
                FaceColor.F => FaceColor.B,
                FaceColor.D => FaceColor.U,
                FaceColor.L => FaceColor.R,
                FaceColor.B => FaceColor.F,
                _ => throw new ArgumentOutOfRangeException(nameof(ce), ce, null)
            };
        }



    }



}
