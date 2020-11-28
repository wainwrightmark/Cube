using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CombinationPuzzle.Coordinate;
using CombinationPuzzle.Data;
using CombinationPuzzle.DataStructures;
using MoreLinq.Extensions;

namespace CombinationPuzzle.Solver
{
    public interface ISolveCoordinator : IDisposable
    {
        bool TryAddSolution(Solution solution);

        int MaxTotalMoves { get; }

        Solution? GetSolution(int? stoppingLength, CancellationToken cancellationToken);

        void MaybeAddSearch(SearchState searchState);

        DataSource DataSource { get; }

    }

    public sealed class SerialSolveCoordinator : ISolveCoordinator
    {
        private readonly object _lock = new object();


        public SerialSolveCoordinator(DataSource dataSource)
        {
            DataSource = dataSource;
        }

        public bool TryAddSolution(Solution solution)
        {
            lock (_lock)
            {
                if (ShortestSolution != null && solution.SolutionMoves.Count >= ShortestSolution.SolutionMoves.Count)
                    return false;
                ShortestSolution = solution;
                MaxTotalMoves = ShortestSolution.SolutionMoves.Count - 1;
                return true;
            }
        }

        public Solution? ShortestSolution { get; private set; }

        public int MaxTotalMoves { get; private set; } = 24;


        public Solution? GetSolution(int? stoppingLength, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && !(ShortestSolution?.SolutionMoves.Count <= stoppingLength) &&  DepthCubes.TryTake(out var searchState))
                searchState.Iterate(this);

            return ShortestSolution;
        }

        public readonly ConcurrentPriorityQueue<SearchState> DepthCubes = new ConcurrentPriorityQueue<SearchState>();
        private readonly ConcurrentDictionary<ImmutableCoordinateCube, int> _seenCubes = new ConcurrentDictionary<ImmutableCoordinateCube, int>();


        public void MaybeAddSearch(SearchState searchState)
        {

            if (_seenCubes.TryAdd(searchState.Cube, searchState.MovesSoFar.Count))
            {
                DepthCubes.Add(searchState);
                return;
            }

            while (true)
            {
                if (!_seenCubes.TryGetValue(searchState.Cube, out var previousMoves) ||
                    previousMoves <= searchState.MovesSoFar.Count) return; //Previous solution is as good as or better


                if (!_seenCubes.TryUpdate(searchState.Cube, searchState.MovesSoFar.Count, previousMoves)) continue; //Failure updating - try again

                DepthCubes.Add(searchState);
                return;
            }
        }

        /// <inheritdoc />
        public DataSource DataSource { get; }

        /// <inheritdoc />
        public void Dispose() => DepthCubes.Dispose();
    }




    public sealed class ParallelSolveCoordinator : ISolveCoordinator
    {
        private readonly object _lock = new object();

        public ParallelSolveCoordinator(int numberOfThreads, DataSource dataSource)
        {
            NumberOfThreads = numberOfThreads;
            DataSource = dataSource;
        }

        public bool TryAddSolution(Solution solution)
        {
            lock (_lock)
            {
                if (ShortestSolution != null && solution.SolutionMoves.Count >= ShortestSolution.SolutionMoves.Count)
                    return false;
                ShortestSolution = solution;
                MaxTotalMoves = ShortestSolution.SolutionMoves.Count - 1;
                _solutions.Add(solution);
                return true;
            }
        }

        public Solution? ShortestSolution { get; private set; }


        public int NumberOfThreads { get; }
        public int MaxTotalMoves { get; private set; } = 24;

        private readonly BlockingCollection<Solution> _solutions = new BlockingCollection<Solution>();

        public Solution? GetSolution(int? stoppingLength, CancellationToken cancellationToken)
        {
            cancellationToken.Register(_solutions.CompleteAdding);

            var tasks = new List<Task>();

            for (var i = 0; i < NumberOfThreads; i++)
                // ReSharper disable AccessToDisposedClosure
                tasks.Add(Task.Run(() =>  SearchForSolution(cancellationToken), cancellationToken));
            // ReSharper restore AccessToDisposedClosure


            var solutions = _solutions.GetConsumingEnumerable(cancellationToken).TakeUntil(x =>
                    stoppingLength.HasValue && x.SolutionMoves.Count <= stoppingLength.Value)
                .ToList();

            var solution = solutions.MinBy(x => x.SolutionMoves.Count).FirstOrDefault();

            return solution;
        }


        private void SearchForSolution( CancellationToken cancellationToken)
        {
            foreach (var item in DepthCubes.GetConsumingEnumerable(cancellationToken))
                item.Iterate(this);

            _solutions.CompleteAdding();
        }

        public readonly BlockingCollection<SearchState> DepthCubes = new BlockingCollection<SearchState>(new ConcurrentPriorityQueue<SearchState>());

        private readonly ConcurrentDictionary<ImmutableCoordinateCube, int> _seenCubes = new ConcurrentDictionary<ImmutableCoordinateCube, int>();


        public void MaybeAddSearch(SearchState searchState)
        {

            if (_seenCubes.TryAdd(searchState.Cube, searchState.MovesSoFar.Count))
            {
                DepthCubes.Add(searchState);
                return;
            }

            while (true)
            {
                if (!_seenCubes.TryGetValue(searchState.Cube, out var previousMoves) ||
                    previousMoves <= searchState.MovesSoFar.Count) return; //Previous solution is as good as or better


                if (!_seenCubes.TryUpdate(searchState.Cube, searchState.MovesSoFar.Count, previousMoves)) continue; //Failure updating - try again

                DepthCubes.Add(searchState);
                return;
            }
        }

        /// <inheritdoc />
        public DataSource DataSource { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            DepthCubes.Dispose();
            _solutions.Dispose();
        }
    }
}