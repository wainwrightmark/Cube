﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using ProtoBuf;

namespace CombinationPuzzle.Data
{
    public abstract class DataSource
    {
        public void WriteToProtoBufDataFile(string folderPath)
        {
            var data = new ProtoBufCubeData()
            {
                CornsliceDepthTable = ByteToByteArray(CornsliceDepthTable),
                UpDownEdgesConjugationTable = UshortToByteArray(UpDownEdgesConjugationTable),
                Phase2PruningTable = UIntToByteArray(Phase2PruningTable),
                Phase2EdgeMergeTable = UshortToByteArray(Phase2EdgeMergeTable),
                Phase1PruningData = UIntToByteArray(Phase1PruningData),
                TwistConj = UshortToByteArray(TwistConj),
                FlipsliceClassIndex = UshortToByteArray(FlipsliceClassIndex),
                FlipsliceSymmetry = ByteToByteArray(FlipsliceSymmetry),
                FlipsliceRep = UIntToByteArray(FlipsliceRep),
                CornerClassIndex = UshortToByteArray(CornerClassIndex),
                CornerSymmetry = ByteToByteArray(CornerSymmetry),
                CornerRep = UshortToByteArray(CornerRep),
                TwistMove = UshortToByteArray(TwistMove),
                FlipMove = UshortToByteArray(FlipMove),
                SliceSortedMove = UshortToByteArray(SliceSortedMove),
                UEdgesMove = UshortToByteArray(UEdgesMove),
                DEdgesMove = UshortToByteArray(DEdgesMove),
                UdEdgesMove = UshortToByteArray(UdEdgesMove),
                CornersMove = UshortToByteArray(CornersMove)
            };

            var path = Path.Combine(folderPath, "ProtoData.data");

            using var stream = File.OpenWrite(path);

            Serializer.Serialize(stream, data);


            static byte[] UIntToByteArray(IEnumerable<uint> enumerable) => enumerable.SelectMany(BitConverter.GetBytes).ToArray();
            static byte[] ByteToByteArray(IEnumerable<byte> enumerable) => enumerable.ToArray();
            static byte[] UshortToByteArray(IEnumerable<ushort> enumerable) => enumerable.SelectMany(BitConverter.GetBytes).ToArray();
        }

        public void WriteToJsonFile(string folderPath)
        {
            var data = new CubeData()
            {
                CornsliceDepthTable = ByteToByteArray(CornsliceDepthTable),
                UpDownEdgesConjugationTable = UshortToByteArray(UpDownEdgesConjugationTable),
                Phase2PruningTable = UIntToByteArray(Phase2PruningTable),
                Phase2EdgeMergeTable = UshortToByteArray(Phase2EdgeMergeTable),
                Phase1PruningData = UIntToByteArray(Phase1PruningData),
                TwistConj = UshortToByteArray(TwistConj),
                FlipsliceClassIndex = UshortToByteArray(FlipsliceClassIndex),
                FlipsliceSymmetry = ByteToByteArray(FlipsliceSymmetry),
                FlipsliceRep = UIntToByteArray(FlipsliceRep),
                CornerClassIndex = UshortToByteArray(CornerClassIndex),
                CornerSymmetry = ByteToByteArray(CornerSymmetry),
                CornerRep = UshortToByteArray(CornerRep),
                TwistMove = UshortToByteArray(TwistMove),
                FlipMove = UshortToByteArray(FlipMove),
                SliceSortedMove = UshortToByteArray(SliceSortedMove),
                UEdgesMove = UshortToByteArray(UEdgesMove),
                DEdgesMove = UshortToByteArray(DEdgesMove),
                UdEdgesMove = UshortToByteArray(UdEdgesMove),
                CornersMove = UshortToByteArray(CornersMove)
            };


            var json = JsonSerializer.Serialize(data);

            File.WriteAllText(Path.Combine(folderPath, "Data.json"), json);

            static byte[] UIntToByteArray(IEnumerable<uint> enumerable) => enumerable.SelectMany(BitConverter.GetBytes).ToArray();
            static byte[] ByteToByteArray(IEnumerable<byte> enumerable) => enumerable.ToArray();
            static byte[] UshortToByteArray(IEnumerable<ushort> enumerable) => enumerable.SelectMany(BitConverter.GetBytes).ToArray();
        }


        public uint GetFlipsliceTwistDepthMod3(ushort sliceSorted, ushort flip, ushort twist)
        {
            var slice = sliceSorted / Definitions.NPerm4;
            var flipslice = Definitions.NFlip * slice + flip;
            var classIndex = FlipsliceClassIndex[flipslice];
            var flipSliceSym = FlipsliceSymmetry[flipslice];

            var twistConj = GetTwistConj(twist, flipSliceSym);

            var ix = Convert.ToInt32(Definitions.NTwist * classIndex + twistConj);

            var y = Phase1PruningData[ix / 16];
            y >>= ix % 16 * 2;
            var r = y & 3;
            return r;
        }

        public ushort GetTwistConj(ushort twist, byte flipSliceSym)
        {
            var r = TwistConj[(twist << 4) + flipSliceSym];

            return r;
        }

        public ushort GetUdEdges(ushort uEdges, ushort dEdges)
        {
            var index = 24 * uEdges + dEdges % 24;
            var r = Phase2EdgeMergeTable[index];

            return r;
        }

        public uint GetCornersUdEdgesDepth3(ushort corners, ushort udEdges)
        {
            var cornerClassIndex = CornerClassIndex[corners];
            var cornerSym = CornerSymmetry[corners];

            var index = Definitions.NUdEdges * cornerClassIndex + GetUdEdgesConj(udEdges, cornerSym);


            var y = Phase2PruningTable[index / 16];
            y >>= index % 16 * 2;
            return y & 3;
        }

        public ushort GetUdEdgesConj(ushort udEdges, byte cornerSym)
        {
            var idx = (udEdges << 4) + cornerSym;

            var r = UpDownEdgesConjugationTable[idx];

            return r;

        }

        public byte GetCornsliceDepth(ushort corners, ushort sliceSorted)
        {
            var index = 24 * corners + sliceSorted;

            var r = CornsliceDepthTable[index];

            return r;
        }


        public abstract ImmutableArray<byte> CornsliceDepthTable { get; }
        public abstract ImmutableArray<ushort> UpDownEdgesConjugationTable { get; }
        public abstract ImmutableArray<uint> Phase2PruningTable { get; }
        public abstract ImmutableArray<ushort> Phase2EdgeMergeTable { get; }
        public abstract ImmutableArray<uint> Phase1PruningData { get; }
        public abstract ImmutableArray<ushort> TwistConj { get; }

        public abstract ImmutableArray<ushort> FlipsliceClassIndex { get; }
        public abstract ImmutableArray<byte> FlipsliceSymmetry { get; }
        public abstract ImmutableArray<uint> FlipsliceRep { get; }


        public abstract ImmutableArray<ushort> CornerClassIndex { get; }
        public abstract ImmutableArray<byte> CornerSymmetry { get; }
        public abstract ImmutableArray<ushort> CornerRep { get; }



        public abstract ImmutableArray<ushort> TwistMove { get; }
        public abstract ImmutableArray<ushort> FlipMove { get; }
        public abstract ImmutableArray<ushort> SliceSortedMove { get; }
        public abstract ImmutableArray<ushort> UEdgesMove { get; }
        public abstract ImmutableArray<ushort> DEdgesMove { get; }
        public abstract ImmutableArray<ushort> UdEdgesMove { get; }
        public abstract ImmutableArray<ushort> CornersMove { get; }
    }
}