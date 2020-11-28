using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CombinationPuzzle.Coordinate;
using CombinationPuzzle.Cubie;
using CombinationPuzzle.Data;
using Medallion.Collections;

namespace CombinationPuzzle.Solver
{
    public static class Solver
    {

        public static Solution? Solve(this ICubieCube cbCube, TimeSpan timeout, int? stoppingLength, DataSource dataSource)
        {
            using var solveCoordinator = new SerialSolveCoordinator(dataSource);
            using var cts = new CancellationTokenSource(timeout);

            var searches = CreateSearches(cbCube, dataSource);

            foreach (var searchState in searches)
                solveCoordinator.MaybeAddSearch(searchState);

            var solution = solveCoordinator.GetSolution(stoppingLength, cts.Token);

            cts.Cancel();

            return solution;
        }

        public static async Task<Solution?> SolveAsync(this ICubieCube cbCube, int numberOfThreads, TimeSpan timeout, int? stoppingLength, DataSource dataSource)
        {
            using var solveCoordinator = new ParallelSolveCoordinator(numberOfThreads, dataSource);
            using var cts = new CancellationTokenSource(timeout);


            var searches = CreateSearches(cbCube, dataSource);

            foreach (var searchState in searches) solveCoordinator.MaybeAddSearch(searchState);

            var solution = solveCoordinator.GetSolution(stoppingLength, cts.Token);

            cts.Cancel();

            await Task.CompletedTask;

            return solution;
        }



        private static IEnumerable<SearchState> CreateSearches(ICubieCube baseCube, DataSource dataSource)
        {
            for (byte rot = 0; rot <= 2; rot++)
            {
                foreach (var inv in new[] { false, true })
                {
                    MutableCubieCube cb;

                    if (rot == 0)
                        // no rotation
                        cb = baseCube.Clone();
                    else if (rot == 1)
                    {
                        // conjugation by 120° rotation
                        cb = Symmetries.Basic.Cubes[32].Clone();
                        cb.Multiply(baseCube);
                        cb.Multiply(Symmetries.Basic.Cubes[16]);
                    }
                    else if (rot == 2)
                    {
                        // conjugation by 240° rotation
                        cb = Symmetries.Basic.Cubes[16].Clone();
                        cb.Multiply(baseCube);
                        cb.Multiply(Symmetries.Basic.Cubes[32]);
                    }
                    else
                        throw new ArgumentException("Rotation should be 1,2, or 3");


                    if (inv)
                        cb = cb.Invert(); // invert cube

                    var coCube = cb.ToCoordinateCube(dataSource);

                    var search = new SearchState(inv, rot, coCube, ImmutableLinkedList<Move>.Empty, false);

                    yield return search;
                }
            }
        }

    }
}