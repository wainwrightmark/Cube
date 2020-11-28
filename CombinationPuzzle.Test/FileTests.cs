using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Data;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace CombinationPuzzle.Test
{
    public sealed class GenericFileTests
    {
        //[Fact(Skip = "Manual")]
        //[Fact]
        public void TestDataSource()
        {
             var dataSource = new ProtoDataSource();

             var path = @"C:\Users\wainw\source\repos\MarkPersonal\CombinationPuzzle\CombinationPuzzle\Data\DataFiles";

             dataSource.WriteToProtoBufDataFile(path);

        }
    }


    public sealed class CubePropertyTester2 : CubePropertyTester
    {
        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(CubePropertyTester))]
        public override void Test(string key) => base.Test(key);

        public CubePropertyTester2(ITestOutputHelper testOutputHelper) => TestOutputHelper = testOutputHelper;
    }


    public class CubePropertyTester : TestBase
    {
        /// <inheritdoc />
        protected override IEnumerable<ITestCase> TestCases => CubeProperty.AllProperties.Select(x => new TestCase(x));


        private class TestCase : ITestCase
        {
            public TestCase(CubeProperty cubeProperty)
            {
                CubeProperty = cubeProperty;
            }

            public CubeProperty CubeProperty { get; }

            /// <inheritdoc />
            public string Name => CubeProperty.GetType().Name;

            /// <inheritdoc />
            public void Execute(ITestOutputHelper testOutputHelper)
            {
                var sw = Stopwatch.StartNew();

                for (ushort i = 0; i < CubeProperty.MaxI; i++)
                {
                    var cube = SolvedCube.Instance.Clone();
                    CubeProperty.SetValue(cube, i);

                    var newValue = CubeProperty.GetValue(cube);

                    newValue.Should().Be(i);
                }

                testOutputHelper.WriteLine(sw.ElapsedMilliseconds + "ms");
            }
        }
    }


    public sealed class FileTester2 : FileTester
    {
        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(FileTester))]
        public override void Test(string key)
        {
            base.Test(key);
        }

        public FileTester2(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }
    }

    public class FileTester : TestBase
    {


        /// <inheritdoc />
        protected override IEnumerable<ITestCase> TestCases
        {
            get
            {
                yield return new TestCase<byte>(nameof(DataSource.CornsliceDepthTable), source => source.CornsliceDepthTable);
                yield return new TestCase<ushort>(nameof(DataSource.UpDownEdgesConjugationTable), source => source.UpDownEdgesConjugationTable);
                yield return new TestCase<uint>(nameof(DataSource.Phase2PruningTable), source => source.Phase2PruningTable);
                yield return new TestCase<ushort>(nameof(DataSource.Phase2EdgeMergeTable), source => source.Phase2EdgeMergeTable);
                yield return new TestCase<uint>(nameof(DataSource.Phase1PruningData), source => source.Phase1PruningData);
                yield return new TestCase<ushort>(nameof(DataSource.TwistConj), source => source.TwistConj);
                yield return new TestCase<ushort>(nameof(DataSource.FlipsliceClassIndex), source => source.FlipsliceClassIndex);
                yield return new TestCase<byte>(nameof(DataSource.FlipsliceSymmetry), source => source.FlipsliceSymmetry);
                yield return new TestCase<uint>(nameof(DataSource.FlipsliceRep), source => source.FlipsliceRep);
                yield return new TestCase<ushort>(nameof(DataSource.CornerClassIndex), source => source.CornerClassIndex);
                yield return new TestCase<byte>(nameof(DataSource.CornerSymmetry), source => source.CornerSymmetry);
                yield return new TestCase<ushort>(nameof(DataSource.CornerRep), source => source.CornerRep);
                yield return new TestCase<ushort>(nameof(DataSource.TwistMove), source => source.TwistMove);
                yield return new TestCase<ushort>(nameof(DataSource.FlipMove), source => source.FlipMove);
                yield return new TestCase<ushort>(nameof(DataSource.SliceSortedMove), source => source.SliceSortedMove);
                yield return new TestCase<ushort>(nameof(DataSource.UEdgesMove), source => source.UEdgesMove);
                yield return new TestCase<ushort>(nameof(DataSource.DEdgesMove), source => source.DEdgesMove);
                yield return new TestCase<ushort>(nameof(DataSource.UdEdgesMove), source => source.UdEdgesMove);
                yield return new TestCase<ushort>(nameof(DataSource.CornersMove), source => source.CornersMove);

            }
        }

        private sealed class TestCase<T> : ITestCase
        {
            public TestCase(string name, Func<DataSource, ImmutableArray<T>> getDataFunc)
            {
                Name = name;
                GetDataFunc = getDataFunc;
            }


            public Func<DataSource, ImmutableArray<T>> GetDataFunc { get; }

            public static DataSource FileDataSource = new ProtoDataSource();

            /// <inheritdoc />
            public string Name { get; }

            /// <inheritdoc />
            public void Execute(ITestOutputHelper testOutputHelper)
            {
                var calculatedDataSource = new DataSourceCalculated();

                var sw = Stopwatch.StartNew();
                var calcData = GetDataFunc(calculatedDataSource);
                sw.Stop();

                testOutputHelper.WriteLine(sw.ElapsedMilliseconds + "ms");
                testOutputHelper.WriteLine(calcData.Length + " rows");

                var fileData = GetDataFunc(FileDataSource);

                calcData.Length.Should().Be(fileData.Length, "arrays should have same length");

                for (var i = 0; i < calcData.Length; i++)
                    calcData[i].Should().Be(fileData[i], $"Arrays should match at index {i}");
            }
        }

        }



}