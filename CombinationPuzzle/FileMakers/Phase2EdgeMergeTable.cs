using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CombinationPuzzle.BasicCubes;
using CombinationPuzzle.Cubie;

namespace CombinationPuzzle.FileMakers
{
    /// <summary>
    /// phase2_edgemerge retrieves the initial phase 2 ud_edges coordinate from the u_edges and d_edges coordinates.
    /// </summary>
    public sealed class Phase2EdgeMergeTable : FileMaker<ushort>
    {
        private Phase2EdgeMergeTable()
        {
        }

        public static ushort GetUdEdges(ushort uEdges, ushort dEdges)
        {
            var index = 24 * uEdges + dEdges % 24;
            var r = Instance.Data[index];

            return r;
        }

        public static FileMaker<ushort> Instance { get; } = new Phase2EdgeMergeTable();

        /// <inheritdoc />
        public override string FileName => "phase2_edgemerge";


        private static readonly ImmutableHashSet<Edge> UEdges = new List<Edge>
        {
            Edge.Ur, Edge.Uf, Edge.Ul, Edge.Ub
        }.ToImmutableHashSet();

        private static readonly ImmutableHashSet<Edge> DEdges = new List<Edge>
        {
            Edge.Dr, Edge.Df, Edge.Dl, Edge.Db
        }.ToImmutableHashSet();

        private static readonly ImmutableArray<Edge> UdEdges = new List<Edge>
        {
            Edge.Ur, Edge.Uf, Edge.Ul, Edge.Ub, Edge.Dr, Edge.Df, Edge.Dl, Edge.Db
        }.ToImmutableArray();

        /// <inheritdoc />
        public override ushort[] Create()
        {
            var cU = SolvedCube.Instance.Clone();
            var cD = SolvedCube.Instance.Clone();
            var cUdEdgePositions = SolvedCube.Instance.EdgePositions.ToArray();

            var cnt = 0;
            var table = new ushort[Definitions.NUEdgesPhase2 * Definitions.NPerm4];

            for (var i = 0; i < Definitions.NUEdgesPhase2; i++)
            {
                cU.set_u_edges(i);
                for (var j = 0; j < Definitions.NChoose84; j++)
                {
                    cD.set_d_edges(j * Definitions.NPerm4);
                    var invalid = false;
                    foreach (var e in UdEdges.Cast<int>())
                    {
                        Edge position;
                        if (UEdges.Contains(cU.EdgePositions[e]))
                            position = cU.EdgePositions[e];
                        else if (DEdges.Contains(cD.EdgePositions[e]))
                            position = cD.EdgePositions[e];
                        else
                        {
                            invalid = true;
                            break;
                        }

                        cUdEdgePositions[e] = position;
                    } //TODO not sure why this is necessary

                    if (invalid) continue;

                    for (var k = 0; k < Definitions.NPerm4; k++)
                    {
                        cD.set_d_edges(j * Definitions.NPerm4 + k);
                        foreach (var e in UdEdges.Cast<int>())
                        {
                            if (UEdges.Contains(cU.EdgePositions[e])) cUdEdgePositions[e] = cU.EdgePositions[e];

                            else if (DEdges.Contains(cD.EdgePositions[e])) cUdEdgePositions[e] = cD.EdgePositions[e];
                        }

                        var cube = new MutableCubieCube(SolvedCube.Instance.CornerPositions.ToArray(), SolvedCube.Instance.CornerOrientations.ToArray(), cUdEdgePositions, SolvedCube.Instance.EdgeOrientations.ToArray());

                        table[Definitions.NPerm4 * i + k] = cube.get_ud_edges();
                        cnt += 1;
                        if (cnt % 2000 == 0) Console.Write(".");
                    }
                }
            }

            return table;
        }
    }
}