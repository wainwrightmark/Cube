using CombinationPuzzle.Symmetries;

namespace CombinationPuzzle.FileMakers
{
    public class CornerRep : FileMaker<ushort>
    {
        private CornerRep()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new CornerRep();

        /// <inheritdoc />
        public override string FileName => "co_rep";

        /// <inheritdoc />
        public override ushort[] Create()
        {
            var r = CornerSymmetries.Create();

            return r.corner_rep;
        }
    }
}