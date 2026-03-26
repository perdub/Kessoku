using Kessoku.Serialization.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kessoku.Serialization.Tests
{
    [Collection("MemoryArrayPoolTests")]
    public class MemoryArrayPoolTests : IDisposable
    {
        [Theory]
        [InlineData(4)]
        [InlineData(1024)]
        [InlineData(4096)]
        [InlineData(6555)]
        [InlineData(8193)]
        public void AllocateBufferWithSize(int size)
        {
            using(MemoryArrayPool mem = new MemoryArrayPool())
            {
                var memArray = mem.RentMemoryArray(size);
                Assert.True(memArray.TotalSize >= size);
                mem.ReturnMemoryArray(memArray);
            }
        }
        public void Dispose()
        {
            
        }
    }
}
