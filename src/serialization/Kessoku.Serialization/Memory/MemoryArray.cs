using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Kessoku.Serialization.Memory
{
    /// <summary>
    /// Please note that you should dispose this to prevent memory leak. we dont use destructor here.
    /// </summary>
    public unsafe class MemoryArray : IDisposable
    {
        private void* memoryPointer;
        private uint size;
        public int TotalSize
        {
            get
            {
                return (int)size;
            }
        }
        public MemoryArray(uint requiredSize, uint align)
        {
            this.size = requiredSize;
#if NET6_0_OR_GREATER
            memoryPointer = NativeMemory.AlignedAlloc(requiredSize, align);
#else
            memoryPointer = Marshal.AllocHGlobal((int)requiredSize).ToPointer();
#endif
        }

        public Span<byte> GetSpan()
        {
            return new Span<byte>(memoryPointer, (int)size);
        }

        public void Dispose()
        {
#if NET6_0_OR_GREATER
            NativeMemory.AlignedFree(memoryPointer);
#else
            Marshal.FreeHGlobal(new IntPtr(memoryPointer));
#endif
        }
    }
}
