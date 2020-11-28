using System;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using FluentAssertions;
using Xunit;

namespace CombinationPuzzle.Test
{
    public class FlipSliceTests
    {
        public static readonly RangeTheoryData RandomSeeds = new RangeTheoryData(0, 50);

        [Fact]
        public void TestFlips()
        {
            for (ushort i = 0; i < Definitions.NFlip; i++)
            {
                var cc = SolvedCube.Instance.Clone();
                cc.set_flip(i);
                var flip = cc.get_flip();
                flip.Should().Be(i);
            }
        }

        [Fact]
        public void TestSlice()
        {
            for (ushort i = 0; i < Definitions.NSlice; i++)
            {
                var cc = SolvedCube.Instance.Clone();

                cc.set_slice(i);

                var slice = cc.get_slice();

                slice.Should().Be(i);
            }
        }


        [Theory]
        [MemberData(nameof(RandomSeeds))]
        public void TestFlipSlice(int seed)
        {
            var c = SolvedCube.Instance.Clone();

            var random = new Random(seed);

            var slice = Convert.ToUInt16(random.Next(Definitions.NSlice));
            var flip = random.Next(Definitions.NFlip);

            c.set_slice(slice);
            c.set_flip(flip);

            flip.Should().Be(flip);
            slice.Should().Be(slice);
        }
    }
}