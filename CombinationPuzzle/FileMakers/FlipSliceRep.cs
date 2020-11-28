using CombinationPuzzle.Symmetries;

namespace CombinationPuzzle.FileMakers
{
    public class FlipSliceRep : FileMaker<uint>
    {
        private FlipSliceRep()
        {
        }

        public static FileMaker<uint> Instance { get; } = new FlipSliceRep();

        /// <inheritdoc />
        public override string FileName => "fs_rep";

        /// <inheritdoc />
        public override uint[] Create()
        {
            var r = FlipSlice.Create();

            return r.rep;
        }
    }
}