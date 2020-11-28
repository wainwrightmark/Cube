namespace CombinationPuzzle.FileMakers
{
    /// <summary>
    /// The twist coordinate describes the 3^7 = 2187 possible orientations of the 8 corners
    /// Twist is between 0 and 2186 inclusive in phase 1,
    /// Twist = 0 in phase 2
    /// </summary>
    /// <returns></returns>
    public class TwistMove : CubieCubePropertyFileMaker
    {
        private TwistMove()
        {
        }

        public static FileMaker<ushort> Instance { get; } = new TwistMove();

        /// <inheritdoc />
        public override string FileName => "move_twist";

        /// <inheritdoc />
        public override CubeProperty CubeProperty => TwistMoveProperty.Instance;
    }
}