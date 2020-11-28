using CombinationPuzzle.Symmetries;

namespace CombinationPuzzle.FileMakers
{
    public class CornerSymmetry : FileMaker<byte>
    {
        private CornerSymmetry()
        {
        }

        public static FileMaker<byte> Instance { get; } = new CornerSymmetry();

        /// <inheritdoc />
        public override string FileName => "co_sym";

        /// <inheritdoc />
        public override byte[] Create()
        {
            var r = CornerSymmetries.Create();

            return r.corner_sym;
        }
    }
}