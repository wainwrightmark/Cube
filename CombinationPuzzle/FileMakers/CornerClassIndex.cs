using CombinationPuzzle.Symmetries;

namespace CombinationPuzzle.FileMakers
{
    public class CornerClassIndex : FileMaker<ushort>
    {
        private CornerClassIndex()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new CornerClassIndex();

        /// <inheritdoc />
        public override string FileName => "co_classidx";

        /// <inheritdoc />
        public override ushort[] Create()
        {
            var r = CornerSymmetries.Create();

            return r.corner_classidx;
        }
    }
}