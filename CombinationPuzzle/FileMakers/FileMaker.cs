using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CombinationPuzzle.FileMakers
{
    /// <summary>
    /// A table that can be calculated or read from a file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FileMaker<T> : IFileMaker
    {
        public abstract string FileName { get; }


        protected FileMaker()
        {
            _lazyData = new Lazy<T[]>(LoadOrCreate);
        }

        private readonly Lazy<T[]> _lazyData;

        public T[] Data => _lazyData.Value;



            /// <inheritdoc />
        IEnumerable<object> IFileMaker.Create()
        {
            var r = Create();

            return r.Cast<object>();
        }

        /// <inheritdoc />
        public IEnumerable<object> ConvertByteArray(byte[] ba)
        {
            return typeof(T).Name switch
            {
                nameof(Byte) => ba.Cast<object>(),
                nameof(UInt16) => Convert(ba, BitConverter.ToUInt16, 2),
                nameof(Int32) => Convert(ba, BitConverter.ToInt32, 4),
                nameof(UInt32) => Convert(ba, BitConverter.ToUInt32, 4),
                _ => throw new ArgumentException($"{typeof(T).Name} not expected")
            };
        }


        private IEnumerable<object> Convert<TO>(byte[] ba, Func<byte[], int, TO> convert, int wordLength)
        {
            for (var i = 0; i < ba.Length; i += wordLength)
            {
                var r = convert(ba, i);
                yield return r!;
            }
        }

        public T[] LoadOrCreate()
        {
            var r = GetFromCacheWithFile(FileName, Create);
            return r;
        }

        public abstract T[] Create();

        /// <inheritdoc />
        public override string ToString()
        {
            return FileName;
        }

        public static T[] GetFromCacheWithFile(string fName, Func<T[]> createFunc)
        {
            return ReadObjectFromFile(fName);

            //if (File.Exists(fName))
            //    return ReadObjectFromFile(fName);

            //Console.WriteLine("creating " + fName + " table...");
            //var obj = createFunc();
            //WriteObjectToFile(fName, obj!);

            //return obj;
        }

        //private static void WriteObjectToFile(string fileName, object obj)
        //{
        //    Console.WriteLine("writing " + fileName + " table...");
        //    using var fs = new FileStream(fileName, FileMode.Create);
        //    var formatter = new BinaryFormatter();
        //    formatter.Serialize(fs, obj);
        //}

        private static T[] ReadObjectFromFile(string fileName)
        {
            Console.WriteLine("loading " + fileName + " table...");

            var data = (byte[]) Resource1.ResourceManager.GetObject(fileName);

            using MemoryStream ms = new MemoryStream(data);
            var bFormatter = new BinaryFormatter();
            var o = bFormatter.Deserialize(ms);
            return (T[])o;
        }
    }
}