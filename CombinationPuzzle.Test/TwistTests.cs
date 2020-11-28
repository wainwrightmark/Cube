using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;
using FluentAssertions;
using Xunit;

namespace CombinationPuzzle.Test
{
    public class TwistTests
    {
        [Fact]
        public void TestTwists()
        {
            for (ushort i = 0; i < Definitions.NTwist; i++)
            {
                var cc = SolvedCube.Instance.Clone();
                cc.set_twist(i);
                var twist = cc.get_twist();
                twist.Should().Be(i);
            }
        }
    }
}