namespace CombinationPuzzle.FileMakers
{
    public class CornersMove : CubieCubePropertyFileMaker
    {
        //#######################################################################################################################
        // ############################ SpecialMoves table for the corners coordinate in phase 2 ########################################
        // The corners coordinate describes the 8! = 40320 permutations of the corners.
        // 0 <= corners < 40320 defined but unused in phase 1, 0 <= corners < 40320 in phase 2, corners = 0 for solved cube

        private CornersMove()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new CornersMove();

        /// <inheritdoc />
        public override string FileName => "move_corners";

        /// <inheritdoc />
        public override CubeProperty CubeProperty => CornersMoveProperty.Instance;
    }
}