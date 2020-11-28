using System;
using System.Collections.Generic;
using System.Linq;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Solver;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace CombinationPuzzle.Test
{
    public class MoveSequenceTests2 : MoveSequenceTests
    {
        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(MoveSequenceTests))]
        public override void Test(string key)
        {
            base.Test(key);
        }

        public MoveSequenceTests2(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }
    }



    public class MoveSequenceTests : TestBase
    {


        private static IEnumerable<TestCase> CreateRandomTestCases()
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

        private static TestCase MakeRandomCube(int numberOfMoves, int seed)
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



            return new TestCase(finalCube, reverseMoves);

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


        private class TestCase : ITestCase
        {
            public TestCase(ICubieCube cube, IReadOnlyList<Move> expectedSteps)
            {
                Cube = cube;
                ExpectedSteps = expectedSteps;
            }

            /// <inheritdoc />
            public string Name => Cube.ToString()!;

            /// <inheritdoc />
            public void Execute(ITestOutputHelper testOutputHelper)
            {
                var cc = Cube.Clone();

                foreach (var expectedStep in ExpectedSteps) cc.Multiply(MoveCubes.BasicCubesByMove[expectedStep]);

                cc.Should().Be(SolvedCube.Instance);
            }

            public ICubieCube Cube { get; }

            public IReadOnlyList<Move> ExpectedSteps { get; }
        }

    }
}