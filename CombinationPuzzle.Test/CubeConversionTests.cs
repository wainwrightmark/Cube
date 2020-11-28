using System;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Facelet;
using FluentAssertions;
using Xunit;

namespace CombinationPuzzle.Test
{
    public class CubeConversionTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void TestCubieFaceletConversion(int seed)
        {
            var random = new Random(seed);


            var cc = SolvedCube.Instance.Clone();

            cc.Randomize(random);

            cc.Verify().ShouldBeSuccessful();

            var fc = cc.ToFaceletCube();

            var toCubieCubeResult = fc.ToCubieCube();

            toCubieCubeResult.ShouldBeSuccessful();

            toCubieCubeResult.Value.Should().Be(cc);

        }


        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //public void TestCubieCoordinateConversion(int seed)
        //{
        //    var random = new Random(seed);


        //    var cc = SolvedCube.Instance.Clone();

        //    cc.Randomize(random);

        //    cc.Verify().ShouldBeSuccessful();

        //    var coordCube = ImmutableCoordinateCube.Create(cc);

        //    var backToCubieCube = coordCube.ToCubieCube();

        //    backToCubieCube.Should().Be(cc);

        //}

    }
}
