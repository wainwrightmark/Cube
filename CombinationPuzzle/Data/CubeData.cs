#pragma warning disable 8618
namespace CombinationPuzzle.Data
{
    /// <summary>
    /// Json serializable CubeData
    /// </summary>
    public class CubeData
    {
        public byte[] CornsliceDepthTable { get; set;}
        public byte[] UpDownEdgesConjugationTable { get; set;}
        public byte[] Phase2PruningTable { get; set;}
        public byte[] Phase2EdgeMergeTable { get; set;}
        public byte[] Phase1PruningData { get; set;}
        public byte[] TwistConj { get; set;}
        public byte[] FlipsliceClassIndex { get; set;}
        public byte[] FlipsliceSymmetry { get; set;}
        public byte[] FlipsliceRep { get; set;}
        public byte[] CornerClassIndex { get; set;}
        public byte[] CornerSymmetry { get; set;}
        public byte[] CornerRep { get; set;}
        public byte[] TwistMove { get; set;}
        public byte[] FlipMove { get; set;}
        public byte[] SliceSortedMove { get; set;}
        public byte[] UEdgesMove { get; set;}
        public byte[] DEdgesMove { get; set;}
        public byte[] UdEdgesMove { get; set;}
        public byte[] CornersMove { get; set;}
    }
}
