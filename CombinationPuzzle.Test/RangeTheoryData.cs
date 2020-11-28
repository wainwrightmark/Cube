using Xunit;

namespace CombinationPuzzle.Test
{
    public class RangeTheoryData : TheoryData<int>
    {
        public RangeTheoryData(int min, int count)
        {
            for (var i = min; i < count; i++) Add(i);
        }


    }
}
