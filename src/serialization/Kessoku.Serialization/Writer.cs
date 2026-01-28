using Kessoku.Serialization.Types;
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kessoku.Serialization
{
    public static class Writer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<TType>(this TType binaryWriter, ref int value) where TType : IBufferWriter<byte>
        {
            var span = binaryWriter.GetSpan(4);
            BinaryPrimitives.WriteInt32LittleEndian(span, value);
            binaryWriter.Advance(4);
        }
    }
}
