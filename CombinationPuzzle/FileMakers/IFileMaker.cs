using System.Collections.Generic;

namespace CombinationPuzzle.FileMakers
{
    public interface IFileMaker
    {
        string FileName { get; }

        IEnumerable<object> Create();

        //IEnumerable<object> LoadFromFile(string path);


        /// <summary>
        /// Converts a byte array to an enumerable of objects of this type
        /// </summary>
        IEnumerable<object> ConvertByteArray(byte[] ba);
    }
}