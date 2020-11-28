namespace CombinationPuzzle.FileMakers
{
    public class DEdgesMove : CubieCubePropertyFileMaker
    {
        private DEdgesMove()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new DEdgesMove();

        //#######################################################################################################################
        // ################# SpecialMoves table for the d_edges coordinate for transition phase 1 -> phase 2 ############################
        // The d_edges coordinate describes the 12!/8! = 11880 possible positions of the DR, DF, DL and DB Edge. It is needed at
        // the end of phase 1 to set up the coordinates of phase 2
        //  0 <= d_edges < 11880 in phase 1, 0 <= d_edges < 1680 in phase 2, d_edges = 0 for solved cube.
        /// <inheritdoc />
        public override string FileName => "move_d_edges";

        /// <inheritdoc />
        public override CubeProperty CubeProperty => DEdgesMoveProperty.Instance;
    }
}