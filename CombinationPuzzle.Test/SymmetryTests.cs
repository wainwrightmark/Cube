using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Symmetries;
using FluentAssertions;
using Xunit;

namespace CombinationPuzzle.Test
{
    public class SymmetryTests
    {
        public static readonly RangeTheoryData SymmetriesData = new RangeTheoryData(0, Basic.Cubes.Count);



        [Theory]
        [MemberData(nameof(SymmetriesData))]
        public void AllSymmetriesShouldHaveSymmetryAtMost12(int i)
        {
            CheckCube(Basic.Cubes[i]);
        }

        private static void CheckCube(ICubieCube symCube)
        {
            var currentCube = symCube.Clone();

            for (var i = 0; i <= 12; i++)
            {
                currentCube.Multiply(symCube);

                if (currentCube.Equals(SolvedCube.Instance))
                    return;
            }

            Assert.True(currentCube.Equals(SolvedCube.Instance), "Cube was not solved after 12 applications");
        }

        [Theory]
        [MemberData(nameof(SymmetriesData))]
        public void InversesShouldMultiplyToSolvedCube(int i)
        {
            var sym = Basic.Cubes[i].Clone();
            var inv = Inverse.GetCube(i);

            sym.Multiply(inv);

            sym.Should().Be(SolvedCube.Instance);

        }
    }
}