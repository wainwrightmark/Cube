using CombinationPuzzle.BasicCubes;
using FluentAssertions;
using Xunit;

namespace CombinationPuzzle.Test
{
    public class BasicTests
    {
        public static readonly TheoryData<ImmutableCubieCube, int> SymmetryGroups = new TheoryData<ImmutableCubieCube, int>()
        {
            {Up.Instance, 4},
            {Down.Instance, 4},
            {Left.Instance, 4},
            {Right.Instance, 4},
            {Front.Instance, 4},
            {Back.Instance, 4},

            {F2Symmetry.Instance, 2},
            {MirrorLr2Symmetry.Instance, 2},
            {U4Symmetry.Instance, 4},
            {Urf3Symmetry.Instance, 3},
        };

        [Theory]
        [MemberData(nameof(SymmetryGroups))]
        public void TestBasicOperationSymmetry(ImmutableCubieCube immutableCubieCube, int symmetry)
        {
            var currentCube = immutableCubieCube.Clone();

            for (var i = 1; i < symmetry; i++)
            {
                currentCube.Should().NotBe(SolvedCube.Instance, $"only {i} applications of {symmetry} were applied");

                currentCube.Multiply(immutableCubieCube);
            }

            currentCube.Should().Be(SolvedCube.Instance, $"Should go back to zero after {symmetry} applications");

        }
    }
}
