using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Data;
using CombinationPuzzle.Facelet;
using CombinationPuzzle.Solver;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace CombinationPuzzle.Test
{
    public class MidDepthSolverTests2 : MidDepthSolverTests
    {
        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(MidDepthSolverTests))]
        public override void Test(string key)
        {
            base.Test(key);
        }

        public MidDepthSolverTests2(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }
    }


    public class MidDepthSolverTests : TestBase
    {

        private static IEnumerable<SolverTestCase> CreateRandomTestCases()
        {
            var seed = 12345;
            const int maxLength = 20;
            const int cubesPerLength = 3;

            for (var i = 2; i <= maxLength; i++)
            {
                for (var j = 0; j < cubesPerLength; j++)
                {
                    var testCase = MakeRandomCube(i, seed);
                    yield return testCase;

                    seed++;
                }
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<ITestCase> TestCases => CreateRandomTestCases();

        private static SolverTestCase MakeRandomCube(int numberOfMoves, int seed)
        {
            var random = new Random(seed);

            var moves = new List<Move>();

            for (var i = 0; i < numberOfMoves; i++)
            {
                var m = CreateMove(random, moves.Any() ? moves.Last() : null as Move?);
                moves.Add(m);
            }

            var c = SolvedCube.Instance.Clone();
            foreach (var moveEnum in moves)
            {
                var m = MoveCubes.BasicCubesByMove[moveEnum];
                c.Multiply(m);
            }

            var reverseMoves = moves.Select(x => x.Inverse()).Reverse().ToList();

            var name = $"({moves.Count})-{Solution.CreateName(reverseMoves, Orientation.Default)}";

            var finalCube = new NamedCubieCube(name, c);


            return new SolverTestCase(finalCube);

            static Move CreateMove(Random r, Move? previous)
            {
                while (true)
                {
                    var m = r.Next(18);
                    var move = (Move)m;

                    if (previous == null || move.CanPrecede(previous.Value)) return move; //The reason this appears backwards is we are going to reverse later
                }
            }
        }
    }


    public class SimpleSolverTests2 : SimpleSolverTests
    {
        public SimpleSolverTests2(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }


        [Theory]
        [ClassData(typeof(SimpleSolverTests))]
        public override void Test(string key)
        {
            base.Test(key);
        }
    }

    public class SimpleSolverTests : TestBase
    {

        /// <inheritdoc />
        protected override IEnumerable<ITestCase> TestCases {
            get
            {
                yield return new SolverTestCase(SolvedCube.Instance);

                foreach (var enumValue in Extensions.GetEnumValues<Move>())
                {
                    var cube = MoveCubes.BasicCubesByMove[enumValue];

                    yield return new SolverTestCase(cube);
                }
            } }

    }


    public class SpecificCubeTests2 : SpecificCubeTests
    {
        public SpecificCubeTests2(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(SpecificCubeTests))]
        public override void Test(string key)
        {
            base.Test(key);
        }
    }


    public class SpecificCubeTests : TestBase
    {
        /// <inheritdoc />
        protected override IEnumerable<ITestCase> TestCases
        {
            get
            {
                var cube1 = FaceletCubeExtensions
                    .CreateImmutableFromString("UDDUUUUUDRLRRRRLBLBFBFFBFFFDDUDDUDDURRRLLLLLLFBFFBBBRB").Value
                    .ToCubieCube().Value;

                yield return new SolverTestCase(cube1){TimeoutMilliseconds = 1000};
            }
        }
    }


    public class LowMoveSolverTests2 : LowMoveSolverTests
    {
        public LowMoveSolverTests2(ITestOutputHelper testOutputHelper) => TestOutputHelper = testOutputHelper;

        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(LowMoveSolverTests))]
        public override void Test(string key) => base.Test(key);
    }


    public class LowMoveSolverTests : TestBase
    {
        public const int NumberOfTests = 100;

        public static readonly int? NumberOfThreads = 6;

        public const int StoppingLength = 21;

        public const int TimeoutMilliseconds = 10000;

        /// <inheritdoc />
        protected override IEnumerable<ITestCase> TestCases
        {
            get
            {
                for (var i = 0; i < NumberOfTests; i++)
                {
                    var random = new Random(i);
                    var cube = SolvedCube.Instance.Clone();

                    cube.Randomize(random);

                    yield return new SolverTestCase(cube) { Threads = NumberOfThreads, StoppingLength = StoppingLength, TimeoutMilliseconds = TimeoutMilliseconds};
                }
            }
        }
    }


    public class MultiThreadSolverTests2 : MultiThreadSolverTests
    {
        public MultiThreadSolverTests2(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(MultiThreadSolverTests))]
        public override void Test(string key)
        {
            base.Test(key);
        }
    }


    public class MultiThreadSolverTests : TestBase
    {
        public const int NumberOfTests = 1000;

        public const int NumberOfThreads = 6;

        /// <inheritdoc />
        protected override IEnumerable<ITestCase> TestCases
        {
            get
            {
                for (var i = 0; i < NumberOfTests; i++)
                {
                    var random = new Random(i);
                    var cube = SolvedCube.Instance.Clone();

                    cube.Randomize(random);

                    yield return new SolverTestCase(cube){Threads = NumberOfThreads};
                }
            }
        }
    }


    public class ComplexSolverTests2 : ComplexSolverTests
    {
        public ComplexSolverTests2(ITestOutputHelper testOutputHelper) => TestOutputHelper = testOutputHelper;

        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(ComplexSolverTests))]
        public override void Test(string key) => base.Test(key);
    }


    public class ComplexSolverTests : TestBase
    {
        public const int NumberOfTests = 100;

        /// <inheritdoc />
        protected override IEnumerable<ITestCase> TestCases {
            get
            {
                for (var i = 0; i < NumberOfTests; i++)
                {
                    var random = new Random(i);
                    var cube = SolvedCube.Instance.Clone();

                    cube.Randomize(random);

                    yield return new SolverTestCase(cube);
                }
            }
        }
    }

    internal sealed class SolverTestCase : ITestCase
    {
        public SolverTestCase(ICubieCube cube)
        {
            Cube = cube;
        }

        public int TimeoutMilliseconds { get; set; } = 1000;

        public int StoppingLength { get; set; } = 24;

        public int? Threads { get; set; } = null;

        public static DataSource DataSource { get; } = new ProtoDataSource();

        /// <inheritdoc />
        public string Name => Cube.ToString()!;

        /// <inheritdoc />
        public void Execute(ITestOutputHelper testOutputHelper)
        {
            Cube.Clone().Verify().ShouldBeSuccessful();

            var fc = Cube.ToFaceletCube();

            testOutputHelper.WriteLine(fc.To2dString());

            testOutputHelper.WriteLine("");

            testOutputHelper.WriteLine(fc.ToString());


            var sw = Stopwatch.StartNew();




            Solution? solution;

            if (Threads.HasValue)
                solution = Cube.SolveAsync(Threads.Value, TimeSpan.FromMilliseconds(TimeoutMilliseconds), StoppingLength, DataSource)
                    .Result;
            else solution = Cube.Solve(TimeSpan.FromMilliseconds(TimeoutMilliseconds), StoppingLength, DataSource);

            var time = sw.ElapsedMilliseconds;

            testOutputHelper.WriteLine($"{time}ms elapsed");

            solution.Should().NotBeNull("A solution should have been found");

            testOutputHelper.WriteLine($"{solution!.SolutionMoves.Count} moves");

            var cc = Cube.Clone();
            foreach (var solutionMove in solution!.SolutionMoves) cc.Multiply(MoveCubes.BasicCubesByMove[solutionMove]);

            cc.Should().Be(SolvedCube.Instance, "The solution should solve the cube");

            solution.SolutionMoves.Count.Should().BeLessOrEqualTo(StoppingLength);
        }

        public ICubieCube Cube { get; }
    }
}