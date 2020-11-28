using CombinationPuzzle.Symmetries;

namespace CombinationPuzzle.FileMakers
{
    public class FlipSliceSymmetry : FileMaker<byte>
    {
        private FlipSliceSymmetry()
        {
        }

        public static FileMaker<byte> Instance { get; } = new FlipSliceSymmetry();

        /// <inheritdoc />
        public override string FileName => "fs_sym";

        /// <inheritdoc />
        public override byte[] Create()
        {
            var r = FlipSlice.Create();

            return r.sym;
        }
    }
}