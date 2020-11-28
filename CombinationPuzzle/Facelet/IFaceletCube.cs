using System.Collections.Generic;

namespace CombinationPuzzle.Facelet
{
    public interface IFaceletCube
    {
        IEnumerable<FaceColor?> Facelets { get; }
        FaceColor? this[int index] { get; }
    }
}