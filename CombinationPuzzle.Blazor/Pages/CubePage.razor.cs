using System;
using System.Linq;
using System.Threading.Tasks;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Data;
using CombinationPuzzle.Facelet;
using CombinationPuzzle.Solver;
using CSharpFunctionalExtensions;

namespace CombinationPuzzle.Blazor.Pages
{
    public partial class CubePage
    {

        public Orientation Orientation { get; private set; } = Orientation.Default;

        public MutableFaceletCube Cube { get; set; } = MutableFaceletCube.SolvedCube;


        public SpecificCube.CubeView MyCubeView { get; set; } = SpecificCube.CubeView.FlatMap;

        public Solution? Solution { get; set; } = null;

        public string Error { get; set; } = "";

        public FaceColor? ChosenColor { get; set; }

        public bool InvertSolution { get; set; }

        public readonly Lazy<DataSource>  DataSource = new Lazy<DataSource>(()=> new JsonDataSource());// new DataSourceCalculated();


        public async Task SetToEmpty()
        {
            Cube = MutableFaceletCube.Empty;
            await CubeHasChanged(null, null);
        }

        public async Task SetToSolved()
        {
            Cube = MutableFaceletCube.SolvedCube;
            await CubeHasChanged(null, null);
        }



        private async Task CubeHasChanged(string? error, Move? moveMade)
        {
            Error = error?? "";

            if (Solution != null && moveMade != null && Solution.SolutionMoves.Any() && Solution.SolutionMoves[0] == moveMade)
                Solution = new Solution(Solution.SolutionMoves.Skip(1).ToList());
            else
            {
                Solution = null;
                await Solve();
            }
        }



        public bool IsNextMove(Move move) => Solution != null && Solution.SolutionMoves.Any() && Solution.SolutionMoves[0] == ChangeOrientation(move);

        private Move ChangeOrientation(Move move)
        {
            var (color, number) = move.Deconstruct();
            var newColor = Orientation.GetNewPosition(color);
            var newOperation = (newColor, number).ToMove();

            return newOperation;
        }

        public async Task ApplyOperation(Move operation)
        {
            var newOperation = ChangeOrientation(operation);

            var cc = Cube.ToCubieCube().Tap(x => x.Verify());

            if (cc.IsSuccess)
            {
                var moveCube = MoveCubes.BasicCubesByMove[newOperation];
                cc.Value.Multiply(moveCube);

                Cube = cc.Value.ToFaceletCube().Clone();
                await CubeHasChanged(null, newOperation);
            }
            else
                 await CubeHasChanged(cc.Error, null);
        }

        public void Rotate(Func<Orientation, Orientation> rotation)
        {
            Orientation = rotation(Orientation);
        }

        public async Task FaceletClicked((FaceColor face, VerticalPositionEnum vertical, HorizontalPositionEnum horizontal) facelet)
        {
            if (facelet.vertical == VerticalPositionEnum.Middle && facelet.horizontal == HorizontalPositionEnum.Middle)
                ChosenColor = facelet.face;
            else
            {
                var currentColor = Cube[facelet];

                if (currentColor != ChosenColor)
                {
                    Cube[facelet] = ChosenColor;
                    await CubeHasChanged(null, null);
                }
            }
        }

        public bool Solving { get; set; } = false;

        public async Task Randomize()
        {
            var cube = SolvedCube.Instance.Clone();
            cube.Randomize(new Random());
            Cube = cube.ToFaceletCube().Clone();
            await CubeHasChanged(null, null);
        }

        public async Task Solve() //TODO allow this to cancel, only show solution from latest version
        {
            if (!Solving)
            {
                Solving = true;
                Solution = null;


                var cubeResult = Cube.ToCubieCube().Tap(x => x.Verify());

                if (cubeResult.IsSuccess)
                {
                    var solution = await Task.Run(()=>cubeResult.Value.Solve(TimeSpan.FromSeconds(1), 22, DataSource.Value)) ;

                    if (solution != null)
                    {
                        Solution = solution;
                        Error = "";
                    }
                    else
                        Error = "No Solution found";

                }
                else
                    Error = cubeResult.Error;

                Solving = false;
            }
        }

    }
}
