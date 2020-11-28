namespace CombinationPuzzle.FileMakers
{
    public class SliceSortedMove : CubieCubePropertyFileMaker
    {
        //#######################################################################################################################
        // ###################### SpecialMoves table for the four UD-slice edges FR, FL, Bl and BR. #####################################
        // The slice_sorted coordinate describes the 12!/8! = 11880 possible positions of the FR, FL, BL and BR Edge.
        // Though for phase 1 only the "unsorted" slice coordinate with Binomial(12,4) = 495 positions is relevant, using the
        // slice_sorted coordinate gives us the permutation of the FR, FL, BL and BR edges at the beginning of phase 2 for free.
        // 0 <= slice_sorted < 11880 in phase 1, 0 <= slice_sorted < 24 in phase 2, slice_sorted = 0 for solved cube

        private SliceSortedMove()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new SliceSortedMove();

        /// <inheritdoc />
        public override string FileName => "move_slice_sorted";


        /// <inheritdoc />
        public override CubeProperty CubeProperty => SliceSortedMoveProperty.Instance;
    }
}