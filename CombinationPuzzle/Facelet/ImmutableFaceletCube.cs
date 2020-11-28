using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CombinationPuzzle.Facelet
{
    public sealed class ImmutableFaceletCube : IFaceletCube
    {

        public readonly ImmutableList<FaceColor> Facelets;


        public ImmutableFaceletCube(IEnumerable<FaceColor> facelets)
        {
            Facelets = facelets.ToImmutableList();
        }

        public static readonly ImmutableFaceletCube SolvedCube = CreateSolved();

        private static ImmutableFaceletCube CreateSolved()
        {
            var facelets = new List<FaceColor>();
            for (var i = 0; i < 9; i++) facelets.Add(FaceColor.U);
            for (var i = 0; i < 9; i++) facelets.Add(FaceColor.R);
            for (var i = 0; i < 9; i++) facelets.Add(FaceColor.F);
            for (var i = 0; i < 9; i++) facelets.Add(FaceColor.D);
            for (var i = 0; i < 9; i++) facelets.Add(FaceColor.L);
            for (var i = 0; i < 9; i++) facelets.Add(FaceColor.B);

            return new ImmutableFaceletCube(facelets);
        }

        public override string ToString()
        {
            return this.Serialize();
        }

        /// <inheritdoc />
        IEnumerable<FaceColor?> IFaceletCube.Facelets => Facelets.Select(x=> x as FaceColor?);

        /// <inheritdoc />
        public FaceColor? this[int index] => Facelets[index];
    }
}
