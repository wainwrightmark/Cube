namespace CombinationPuzzle.FileMakers
{
    public class UDEdgesMove : CubieCubePropertyFileMaker
    {
        //#######################################################################################################################
        // ######################### # SpecialMoves table for the edges in the U-face and D-face. #######################################
        // The ud_edges coordinate describes the 40320 permutations of the edges UR, UF, UL, UB, DR, DF, DL and DB in phase 2
        // ud_edges undefined in phase 1, 0 <= ud_edges < 40320 in phase 2, ud_edges = 0 for solved cube.

        private UDEdgesMove()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new UDEdgesMove();

        /// <inheritdoc />
        public override string FileName => "move_ud_edges";

        /// <inheritdoc />
        public override CubeProperty CubeProperty => UDEdgesMoveProperty.Instance;
    }
}