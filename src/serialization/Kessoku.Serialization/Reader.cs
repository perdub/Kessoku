using Kessoku.Serialization.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kessoku.Serialization
{
    public static class Reader
    {
#if NET8_0_OR_GREATER
        public static void Read(this BinaryReader binaryReader, out Int128 value)
        {
            value = new Int128(binaryReader.ReadUInt64(), binaryReader.ReadUInt64());
        }
        public static void Read(this BinaryReader binaryReader, out UInt128 value)
        {
            value = new UInt128(binaryReader.ReadUInt64(), binaryReader.ReadUInt64());
        }
#endif
        public static void Read(this BinaryReader binaryReader, out int value)
        {
            value = binaryReader.ReadInt32();
        }

        public static void Read(this BinaryReader binaryReader, out long value)
        {
            value = binaryReader.ReadInt64();
        }

        public static void Read(this BinaryReader binaryReader, out string value)
        {
            value = binaryReader.ReadString();
        }

        public static void Read(this BinaryReader binaryReader, out float value)
        {
            value = binaryReader.ReadSingle();
        }

        public static void Read(this BinaryReader binaryReader, out double value)
        {
            value = binaryReader.ReadDouble();
        }

        public static void Read(this BinaryReader binaryReader, out byte value)
        {
            value = binaryReader.ReadByte();
        }

        public static void Read(this BinaryReader binaryReader, out uint value)
        {
            value = binaryReader.ReadUInt32();
        }

        public static unsafe void Read(this BinaryReader binaryReader, out Guid guid)
        {
#if NET8_0_OR_GREATER
            Span<byte> mem = stackalloc byte[16];
            binaryReader.Read(mem);
            guid = new Guid(mem);
#else
            guid = new Guid(binaryReader.ReadBytes(16));
#endif
        }

        public static void Read(this BinaryReader binaryReader, out byte[] bytes)
        {
            int length = binaryReader.ReadInt32();
            bytes = binaryReader.ReadBytes(length);
        }

        public static void Read<T>(this BinaryReader binaryReader, out T value) where T : ISerialization, new()
        {
            T t = new T();
            t.Deserialize(binaryReader);
            value = t;
        }

        public static void Read<T>(this BinaryReader binaryReader, out T[] value) where T : ISerialization, new()
        {
            int length = binaryReader.ReadInt32();
            T[] array = new T[length];
            for (int i = 0; i < length; i++)
            {
                var tempObj = new T();
                tempObj.Deserialize(binaryReader);
                array[i] = tempObj;
            }

            value = array;
        }
    }
}
