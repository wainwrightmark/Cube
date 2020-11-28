using CombinationPuzzle.Symmetries;

namespace CombinationPuzzle.FileMakers
{
    public class FlipSliceClass : FileMaker<ushort>
    {
        private FlipSliceClass()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new FlipSliceClass();

        /// <inheritdoc />
        public override string FileName => "fs_classidx";

        /// <inheritdoc />
        public override ushort[] Create()
        {
            var r = FlipSlice.Create();

            return r.classIndex;
        }
    }
}