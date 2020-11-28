using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CombinationPuzzle.Cubie;

namespace CombinationPuzzle.BasicCubes
{

    public class NamedCubieCube : ImmutableCubieCube
    {
        /// <inheritdoc />
        public NamedCubieCube(string name, ICubieCube cube)
        {
            Name = name;
            CornerOrientations = cube.CornerOrientations.ToImmutableArray();
            CornerPositions = cube.CornerPositions.ToImmutableArray();
            EdgeOrientations = cube.EdgeOrientations.ToImmutableArray();
            EdgePositions = cube.EdgePositions.ToImmutableArray();
        }

        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        public override ImmutableArray<Corner> CornerPositions { get; }

        /// <inheritdoc />
        public override ImmutableArray<int> EdgeOrientations { get; }



        /// <inheritdoc />
        public override ImmutableArray<int> CornerOrientations { get; }

        /// <inheritdoc />
        public override ImmutableArray<Edge> EdgePositions { get; }
    }

    public abstract class ImmutableCubieCube : ICubieCube, IEquatable<ICubieCube>
    {
        /// <inheritdoc />
        IReadOnlyList<int> ICubieCube.EdgeOrientations => EdgeOrientations;

        /// <inheritdoc />
        IReadOnlyList<Edge> ICubieCube.EdgePositions => EdgePositions;
        /// <inheritdoc />
        IReadOnlyList<int> ICubieCube.CornerOrientations => CornerOrientations;

        /// <inheritdoc />
        IReadOnlyList<Corner> ICubieCube.CornerPositions => CornerPositions;

        /// <inheritdoc />
        public MutableCubieCube Clone()
        {
            return new MutableCubieCube(CornerPositions.ToArray(), CornerOrientations.ToArray(), EdgePositions.ToArray(), EdgeOrientations.ToArray());
        }

        public abstract ImmutableArray<Corner> CornerPositions { get; }

        public abstract ImmutableArray<int> CornerOrientations { get; }

        public abstract ImmutableArray<Edge> EdgePositions { get; }

        public abstract ImmutableArray<int> EdgeOrientations { get; }

        public ImmutableArray<int> ZeroCornerOrientation = Enumerable.Repeat(0,8).ToImmutableArray();
        public ImmutableArray<int> ZeroEdgeOrientation = Enumerable.Repeat(0,12).ToImmutableArray();


        public abstract string Name { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        /// <inheritdoc />
        public bool Equals(ICubieCube? other) => other != null && CubeComparer.Instance.Equals(this, other);

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ImmutableCubieCube)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return CubeComparer.Instance.GetHashCode(this);
        }

        public static bool operator ==(ImmutableCubieCube? left, ImmutableCubieCube? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ImmutableCubieCube? left, ImmutableCubieCube? right)
        {
            return !Equals(left, right);
        }

    }
}