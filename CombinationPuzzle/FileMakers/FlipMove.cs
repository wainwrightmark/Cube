namespace CombinationPuzzle.FileMakers
{
    public class FlipMove : CubieCubePropertyFileMaker
    {
        // The flip coordinate describes the 2^11 = 2048 possible orientations of the 12 edges
        // 0 <= flip < 2048 in phase 1, flip = 0 in phase 2

        private FlipMove()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new FlipMove();

        ///// <inheritdoc />
        public override string FileName => "move_flip";

        /// <inheritdoc />
        public override CubeProperty CubeProperty => FlipMoveProperty.Instance;
    }
}