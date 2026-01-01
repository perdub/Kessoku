using Kessoku.Serialization.Types;
using System;
using System.IO;

namespace Kessoku.Serialization
{
    public static class Writer
    {
        public static void Write(this BinaryWriter binaryWriter, int value)
        {
            binaryWriter.Write(value);
        }
#if NET8_0_OR_GREATER
        public static unsafe void Write(this BinaryWriter binaryWriter, UInt128 value)
        {
            var ptr = &value;
            binaryWriter.Write(new ReadOnlySpan<byte>(
                (void*)ptr, 16
                ));
        }
        public static unsafe void Write(this BinaryWriter binaryWriter, Int128 value)
        {
            var ptr = &value;
            binaryWriter.Write(new ReadOnlySpan<byte>(
                (void*)ptr, 16
            ));
        }
#endif
        /*  public static void Write(this BinaryWriter binaryWriter, ISerialization value)
          {
              value.Serialize(binaryWriter);
          }*/

        public static void Write(this BinaryWriter binaryWriter, Guid guid)
        {
            if (binaryWriter is KessokuBinaryWriter aeeBinaryWriter)
            {
                aeeBinaryWriter.Write(guid);
            }
            else
            {
                binaryWriter.Write(guid.ToByteArray());
            }
        }

        public static void Write(this BinaryWriter binaryWriter, uint value)
        {
            binaryWriter.Write(value);
        }

        public static void Write<T>(this BinaryWriter binaryWriter, T value) where T : ISerialization
        {
            value.Serialize(binaryWriter);
        }

        public static void Write<T>(this BinaryWriter binaryWriter, T[] value) where T : ISerialization
        {
            binaryWriter.Write(value.Length);
            foreach (var val in value)
            {
                val.Serialize(binaryWriter);
            }
        }
    }
}
