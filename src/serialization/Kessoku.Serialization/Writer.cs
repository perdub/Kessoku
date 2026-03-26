using Kessoku.Serialization.Types;
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kessoku.Serialization
{
    public ref struct Writer
    {
        public Writer(IBufferWriter<byte> buffer)
        {
            WriteSpan = buffer.GetSpan();
            TotalAdvanced = 0;
        }

        public Span<byte> WriteSpan { get; set; }
        public int TotalAdvanced { get; set; }

        public unsafe void Write<T>(T unmanag)
        {
            var localSpan = WriteSpan;
#if NET5_0_OR_GREATER
            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                ref byte b = ref MemoryMarshal.GetReference(WriteSpan);
                Unsafe.Copy((void*)b, ref unmanag);
                TotalAdvanced += Unsafe.SizeOf<T>();
            }
#endif
        }
    }
}
