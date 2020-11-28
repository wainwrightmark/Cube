using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json;

namespace CombinationPuzzle.Data
{
    public sealed class JsonDataSource : DataSource
    {
        /// <inheritdoc />
        public JsonDataSource()
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Beginning Loading of Cube Data");

            var cubeData = JsonSerializer.Deserialize<CubeData>(CubeDataResource.Data)!;

            CornsliceDepthTable = (cubeData.CornsliceDepthTable).ToImmutableArray();
            UpDownEdgesConjugationTable = UShortsFromByteArray(cubeData.UpDownEdgesConjugationTable).ToImmutableArray();
            Phase2PruningTable = UIntsFromByteArray(cubeData.Phase2PruningTable).ToImmutableArray();
            Phase2EdgeMergeTable = UShortsFromByteArray(cubeData.Phase2EdgeMergeTable).ToImmutableArray();
            Phase1PruningData = UIntsFromByteArray(cubeData.Phase1PruningData).ToImmutableArray();
            TwistConj = UShortsFromByteArray(cubeData.TwistConj).ToImmutableArray();
            FlipsliceClassIndex = UShortsFromByteArray(cubeData.FlipsliceClassIndex).ToImmutableArray();
            FlipsliceSymmetry = (cubeData.FlipsliceSymmetry).ToImmutableArray();
            FlipsliceRep = UIntsFromByteArray(cubeData.FlipsliceRep).ToImmutableArray();
            CornerClassIndex = UShortsFromByteArray(cubeData.CornerClassIndex).ToImmutableArray();
            CornerSymmetry = (cubeData.CornerSymmetry).ToImmutableArray();
            CornerRep = UShortsFromByteArray(cubeData.CornerRep).ToImmutableArray();
            TwistMove = UShortsFromByteArray(cubeData.TwistMove).ToImmutableArray();
            FlipMove = UShortsFromByteArray(cubeData.FlipMove).ToImmutableArray();
            SliceSortedMove = UShortsFromByteArray(cubeData.SliceSortedMove).ToImmutableArray();
            UEdgesMove = UShortsFromByteArray(cubeData.UEdgesMove).ToImmutableArray();
            DEdgesMove = UShortsFromByteArray(cubeData.DEdgesMove).ToImmutableArray();
            UdEdgesMove =  UShortsFromByteArray(cubeData.UdEdgesMove).ToImmutableArray();
            CornersMove = UShortsFromByteArray(cubeData.CornersMove) .ToImmutableArray();

            Console.WriteLine($"Loading of Cube Data complete ({sw.Elapsed})");

            static IEnumerable<ushort> UShortsFromByteArray(byte[] bytes)
            {
                for (var i = 0; i < bytes.Length - 1; i += 2)
                {
                    var us = BitConverter.ToUInt16(bytes, i);
                    yield return us;
                }
            }

            static IEnumerable<uint> UIntsFromByteArray(byte[] bytes)
            {
                for (var i = 0; i < bytes.Length - 3; i += 4)
                {
                    var us = BitConverter.ToUInt32(bytes, i);
                    yield return us;
                }
            }
        }



        /// <inheritdoc />
        public override ImmutableArray<byte> CornsliceDepthTable { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> UpDownEdgesConjugationTable { get; }

        /// <inheritdoc />
        public override ImmutableArray<uint> Phase2PruningTable { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> Phase2EdgeMergeTable { get; }

        /// <inheritdoc />
        public override ImmutableArray<uint> Phase1PruningData { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> TwistConj { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> FlipsliceClassIndex { get; }

        /// <inheritdoc />
        public override ImmutableArray<byte> FlipsliceSymmetry { get; }

        /// <inheritdoc />
        public override ImmutableArray<uint> FlipsliceRep { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> CornerClassIndex { get; }

        /// <inheritdoc />
        public override ImmutableArray<byte> CornerSymmetry { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> CornerRep { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> TwistMove { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> FlipMove { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> SliceSortedMove { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> UEdgesMove { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> DEdgesMove { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> UdEdgesMove { get; }

        /// <inheritdoc />
        public override ImmutableArray<ushort> CornersMove { get; }
    }
}