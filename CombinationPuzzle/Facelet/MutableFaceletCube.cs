using System;
using System.Collections.Generic;
using System.Linq;

namespace CombinationPuzzle.Facelet
{
    public sealed class MutableFaceletCube : IFaceletCube
    {
        public MutableFaceletCube(IEnumerable<FaceColor?> facelets)
        {
            _facelets = facelets.Take(54).ToArray();

            if(_facelets.Length != 54)
                throw new Exception("Facelet cube must have 54 facelets");
        }


        private readonly FaceColor?[] _facelets;

        public FaceColor? this[(FaceColor face, VerticalPositionEnum row, HorizontalPositionEnum column) index]
        {
            get => _facelets[ConvertToInt(index)];
            set => _facelets[ConvertToInt(index)] = value;
        }

        private static int ConvertToInt((FaceColor face, VerticalPositionEnum row, HorizontalPositionEnum column) index)
        {
            return (int) index.face * 9 + (int)index.row * 3 + (int) index.column;
        }


        /// <inheritdoc />
        public IEnumerable<FaceColor?> Facelets => _facelets;

        /// <inheritdoc />
        public FaceColor? this[int index] => _facelets[index];


        public static MutableFaceletCube SolvedCube => ImmutableFaceletCube.SolvedCube.Clone();

        public static MutableFaceletCube Empty => new MutableFaceletCube(EmptyFacelets);

        private static IEnumerable<FaceColor?> EmptyFacelets
        {
            get
            {
                var facelets = new FaceColor?[54];

                //set centers
                foreach (var c in Extensions.GetEnumValues<FaceColor>())
                {
                    var centre = (int)c * 9 + 4;
                    facelets[centre] = c;
                }

                return facelets;
            }
        }
    }
}