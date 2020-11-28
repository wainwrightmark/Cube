using ProtoBuf;

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

    [ProtoContract(SkipConstructor = true)]

    public class ProtoBufCubeData
    {

        [ProtoMember(1)]  public byte[] CornsliceDepthTable { get; set; }
        [ProtoMember(2)]  public byte[] UpDownEdgesConjugationTable { get; set; }
        [ProtoMember(3)]  public byte[] Phase2PruningTable { get; set; }
        [ProtoMember(4)]  public byte[] Phase2EdgeMergeTable { get; set; }
        [ProtoMember(5)]  public byte[] Phase1PruningData { get; set; }
        [ProtoMember(6)]  public byte[] TwistConj { get; set; }
        [ProtoMember(7)]  public byte[] FlipsliceClassIndex { get; set; }
        [ProtoMember(8)]  public byte[] FlipsliceSymmetry { get; set; }
        [ProtoMember(9)]  public byte[] FlipsliceRep { get; set; }
        [ProtoMember(10)]  public byte[] CornerClassIndex { get; set; }
        [ProtoMember(11)]  public byte[] CornerSymmetry { get; set; }
        [ProtoMember(12)]  public byte[] CornerRep { get; set; }
        [ProtoMember(13)]  public byte[] TwistMove { get; set; }
        [ProtoMember(14)]  public byte[] FlipMove { get; set; }
        [ProtoMember(15)]  public byte[] SliceSortedMove { get; set; }
        [ProtoMember(16)]  public byte[] UEdgesMove { get; set; }
        [ProtoMember(17)]  public byte[] DEdgesMove { get; set; }
        [ProtoMember(18)]  public byte[] UdEdgesMove { get; set; }
        [ProtoMember(19)]  public byte[] CornersMove { get; set; }
    }

}
