namespace CombinationPuzzle.FileMakers
{
    public class UEdgesMove : CubieCubePropertyFileMaker
    {
        //#######################################################################################################################
        // ################# SpecialMoves table for the u_edges coordinate for transition phase 1 -> phase 2 ############################
        // The u_edges coordinate describes the 12!/8! = 11880 possible positions of the UR, UF, UL and UB Edge. It is needed at
        // the end of phase 1 to set up the coordinates of phase 2
        // 0 <= u_edges < 11880 in phase 1, 0 <= u_edges < 1680 in phase 2, u_edges = 1656 for solved cube."""

        private UEdgesMove()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new UEdgesMove();

        /// <inheritdoc />
        public override string FileName => "move_u_edges";


        /// <inheritdoc />
        public override CubeProperty CubeProperty => UEdgesMovesProperty.Instance;
    }
}