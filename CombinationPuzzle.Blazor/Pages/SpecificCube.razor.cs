using System;
using System.Threading.Tasks;
using CombinationPuzzle.Facelet;
using Microsoft.AspNetCore.Components;

namespace CombinationPuzzle.Blazor.Pages
{
    public partial class SpecificCube
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
#pragma warning disable 649
        const int SizeUnit = 25;
        private string _scene;

        private string _cube;
        private string _cubeFace;
        private string _square;
#pragma warning restore 649
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [Parameter] public MutableFaceletCube Cube { get; set; } = null!;

        [Parameter] public Orientation Orientation { get; set; } = null!;

        [Parameter] public CubeView MyCubeView { get; set; } = CubeView.FlatMap;
        [Parameter] public EventCallback<(FaceColor, VerticalPositionEnum, HorizontalPositionEnum)> OnFaceletClick { get; set; }


        public double RotateX { get; set; } = -30;
        public double RotateY { get; set; } = -45;
        public double RotateZ { get; set; } = 0;

        //public string Uri => $"cube/{Cube.Serialize()}";



        const int ExplodedTranslateUnits = 12;
        const int CompactTranslateUnits = 3;

        public async Task FaceletClicked(FaceColor color, VerticalPositionEnum verticalPosition, HorizontalPositionEnum horizontalPosition)
        {
            await OnFaceletClick.InvokeAsync((color, verticalPosition, horizontalPosition));
        }

        private string GetCubeStyle()
        {
            switch (MyCubeView)
            {
                case CubeView.Compact3D:
                case CubeView.Exploded3D:
                    return $"transform: rotateX({UnitsInDegrees(RotateX)}) rotateY({UnitsInDegrees(RotateY)}) rotateZ({UnitsInDegrees(RotateZ)});";
                case CubeView.FlatMap:
                    return "transform: rotateX(0) rotateY(0) rotateZ(0);";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private static string GetFaceStyle(FaceColor color, CubeView cubeView, Orientation orientation)
        {

            var orientationColor = orientation.GetInversePosition(color);

            if (cubeView == CubeView.Compact3D || cubeView == CubeView.Exploded3D)
            {
                var exploded = cubeView == CubeView.Exploded3D;
                return orientationColor switch
                {
                    FaceColor.U => $"transform: rotateX(90deg) translate3d(0, 0, {UnitsInPixels(3)});",
                    FaceColor.L => $"transform: rotateY(-90deg) translate3d(0, 0, {UnitsInPixels(exploded ? ExplodedTranslateUnits : CompactTranslateUnits)});",
                    FaceColor.F => $"transform: translate3d(0, 0, {UnitsInPixels(3)});",
                    FaceColor.R => $"transform: rotateY(90deg) translate3d(0, 0, {UnitsInPixels(3)});",
                    FaceColor.B => $"transform: rotateY(180deg) translate3d(0, 0, {UnitsInPixels(exploded ? ExplodedTranslateUnits : CompactTranslateUnits)});",
                    FaceColor.D => $"transform: rotateX(-90deg) translate3d(0, 0, {UnitsInPixels(exploded ? ExplodedTranslateUnits : CompactTranslateUnits)});",
                    _ => throw new ArgumentOutOfRangeException(nameof(orientationColor), orientationColor, null)
                };
            }
            else if(cubeView == CubeView.FlatMap)
            {
                return orientationColor switch
                {
                    FaceColor.U => $"transform: translate(0, {UnitsInPixels(-6)});",
                    FaceColor.L => $"transform: translate({UnitsInPixels(-6)}, 0);",
                    FaceColor.F => $"transform: translate(0, 0);",
                    FaceColor.R => $"transform: translate({UnitsInPixels(6)}, 0);",
                    FaceColor.B => $"transform: translate({UnitsInPixels(12)}, 0);",
                    FaceColor.D => $"transform: translate(0, {UnitsInPixels(6)});",
                    _ => throw new ArgumentOutOfRangeException(nameof(orientationColor), orientationColor, null)
                };
            }

            throw new ArgumentOutOfRangeException(nameof(cubeView), cubeView, null);

        }

        private static string UnitsInPixels(int multiple) => SizeUnit * multiple + "px";

        private static string UnitsInDegrees(double d) => d + "deg";


        public enum CubeView
        {
            FlatMap,
            Compact3D,
            Exploded3D
        }
    }
}
