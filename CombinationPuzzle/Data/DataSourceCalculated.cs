using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.DataMakers;
using CombinationPuzzle.Symmetries;

namespace CombinationPuzzle.Data
{
    public sealed class DataSourceCalculated : DataSource
    {
        /// <inheritdoc />
        public DataSourceCalculated()
        {
            _cornsliceDepthTableLazy = new Lazy<ImmutableArray<byte>>(()=>CornsliceDepth.Create(this));
            _upDownEdgesConjugationTableLazy = new Lazy<ImmutableArray<ushort>>(UpDownEdgesConjugation.Create);
            _phase2PruningTableLazy = new Lazy<ImmutableArray<uint>>(() => Phase2Pruning.Create(this));
            _phase2EdgeMergeTableLazy = new Lazy<ImmutableArray<ushort>>(Phase2EdgeMerge.Create);
            _phase1PruningDataLazy = new Lazy<ImmutableArray<uint>>(() => Phase1Pruning.Create(this));
            _flipSliceLazy = new Lazy<(ImmutableArray<ushort> classIndex, ImmutableArray<byte> sym, ImmutableArray<uint> rep)>(FlipSlice.Create);
            _cornerSymmetriesLazy = new Lazy<(ImmutableArray<ushort> corner_classidx, ImmutableArray<byte> corner_sym, ImmutableArray<ushort> corner_rep)>(CornerSymmetries.Create);

            _twistMoveLazy = new Lazy<ImmutableArray<ushort>>(Create(TwistMoveProperty.Instance));

            _flipMoveLazy = new Lazy<ImmutableArray<ushort>>(Create(FlipMoveProperty.Instance));
            _sliceSortedMoveLazy = new Lazy<ImmutableArray<ushort>>(Create(SliceSortedMoveProperty.Instance));
            _uEdgesMoveLazy = new Lazy<ImmutableArray<ushort>>(Create(UEdgesMovesProperty.Instance));
            _dEdgesMoveLazy = new Lazy<ImmutableArray<ushort>>(Create(DEdgesMoveProperty.Instance));
            _udEdgesMoveLazy = new Lazy<ImmutableArray<ushort>>(Create(UDEdgesMoveProperty.Instance));
            _cornersMoveLazy = new Lazy<ImmutableArray<ushort>>(Create(CornersMoveProperty.Instance));

        }

        public void WriteToFolder(string folderPath)
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



        private readonly Lazy<ImmutableArray<byte>> _cornsliceDepthTableLazy;

        /// <inheritdoc />
        public override ImmutableArray<byte> CornsliceDepthTable => _cornsliceDepthTableLazy.Value;
        private readonly Lazy<ImmutableArray<ushort>> _upDownEdgesConjugationTableLazy;

        /// <inheritdoc />
        public override ImmutableArray<ushort> UpDownEdgesConjugationTable => _upDownEdgesConjugationTableLazy.Value;

        private readonly Lazy<ImmutableArray<uint>> _phase2PruningTableLazy;

        /// <inheritdoc />
        public override ImmutableArray<uint> Phase2PruningTable => _phase2PruningTableLazy.Value;

        private readonly Lazy<ImmutableArray<ushort>> _phase2EdgeMergeTableLazy;

        /// <inheritdoc />
        public override ImmutableArray<ushort> Phase2EdgeMergeTable => _phase2EdgeMergeTableLazy.Value;

        private readonly Lazy<ImmutableArray<uint>> _phase1PruningDataLazy;

        /// <inheritdoc />
        public override ImmutableArray<uint> Phase1PruningData => _phase1PruningDataLazy.Value;


        private readonly Lazy<ImmutableArray<ushort>> _twistConjLazy = new Lazy<ImmutableArray<ushort>>(TwistConjugation.CreateTable);

        /// <inheritdoc />
        public override ImmutableArray<ushort> TwistConj => _twistConjLazy.Value;

        private readonly Lazy<(ImmutableArray<ushort> classIndex, ImmutableArray<byte> sym, ImmutableArray<uint> rep)> _flipSliceLazy;


        /// <inheritdoc />
        public override ImmutableArray<ushort> FlipsliceClassIndex => _flipSliceLazy.Value.classIndex;

        /// <inheritdoc />
        public override ImmutableArray<byte> FlipsliceSymmetry => _flipSliceLazy.Value.sym;

        /// <inheritdoc />
        public override ImmutableArray<uint> FlipsliceRep => _flipSliceLazy.Value.rep;


        private readonly Lazy<(ImmutableArray<ushort> corner_classidx, ImmutableArray<byte> corner_sym,ImmutableArray<ushort> corner_rep)> _cornerSymmetriesLazy;

        /// <inheritdoc />
        public override ImmutableArray<ushort> CornerClassIndex => _cornerSymmetriesLazy.Value.corner_classidx;

        /// <inheritdoc />
        public override ImmutableArray<byte> CornerSymmetry => _cornerSymmetriesLazy.Value.corner_sym;

        /// <inheritdoc />
        public override ImmutableArray<ushort> CornerRep => _cornerSymmetriesLazy.Value.corner_rep;



        private readonly Lazy<ImmutableArray<ushort>> _twistMoveLazy;
        private readonly Lazy<ImmutableArray<ushort>> _flipMoveLazy;
        private readonly Lazy<ImmutableArray<ushort>> _sliceSortedMoveLazy;
        private readonly Lazy<ImmutableArray<ushort>> _uEdgesMoveLazy;
        private readonly Lazy<ImmutableArray<ushort>> _dEdgesMoveLazy;
        private readonly Lazy<ImmutableArray<ushort>> _udEdgesMoveLazy;
        private readonly Lazy<ImmutableArray<ushort>> _cornersMoveLazy;

        /// <inheritdoc />
        public override ImmutableArray<ushort> TwistMove => _twistMoveLazy.Value;

        /// <inheritdoc />
        public override ImmutableArray<ushort> FlipMove => _flipMoveLazy.Value;

        /// <inheritdoc />
        public override ImmutableArray<ushort> SliceSortedMove => _sliceSortedMoveLazy.Value;

        /// <inheritdoc />
        public override ImmutableArray<ushort> UEdgesMove => _uEdgesMoveLazy.Value;

        /// <inheritdoc />
        public override ImmutableArray<ushort> DEdgesMove => _dEdgesMoveLazy.Value;

        /// <inheritdoc />
        public override ImmutableArray<ushort> UdEdgesMove => _udEdgesMoveLazy.Value;

        /// <inheritdoc />
        public override ImmutableArray<ushort> CornersMove => _cornersMoveLazy.Value;


        private static  ImmutableArray<ushort> Create(CubeProperty cubeProperty)
        {
            var resultTable = new ushort[cubeProperty.MaxI * Definitions.NMove];
            for (ushort i = 0; i < cubeProperty.MaxI; i++)
            {
                var a = SolvedCube.Instance.Clone();
                cubeProperty.SetValue(a, i);

                foreach (var move in MoveExtensions.AllMoves)
                {
                    if (cubeProperty.MovesToIgnore != null && cubeProperty.MovesToIgnore.Contains(move)) continue;
                    var multiplyCube = MoveCubes.BasicCubesByMove[move];
                    var a2 = a.Clone();
                    if (cubeProperty.IsEdges)
                        a2.EdgeMultiply(multiplyCube);
                    else
                        a2.CornerMultiply(multiplyCube);

                    var newValue = cubeProperty.GetValue(a2);
                    resultTable[Definitions.NMove * i + (int)move] = newValue;
                }
            }

            return resultTable.ToImmutableArray();
        }


    }
}