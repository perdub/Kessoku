using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Kessoku.Serialization.Memory
{
    public class MemoryArrayPool : IDisposable
    {
        const uint MINIMAL_MEMORY_ARRAY_SIZE = 4096;
        const uint ALIGN_MEMORY = 4096;
        const uint MAX_BAG_ELEMENT_COUNT = 8;

        private Lazy<ConcurrentBag<MemoryArray>>[] pool;
        private int[] poolSizes;
        public MemoryArrayPool()
        {
            pool = new Lazy<ConcurrentBag<MemoryArray>>[32];
            poolSizes = new int[pool.Length];
            for (int i = 0; i < pool.Length; i++) {
                pool[i] = new Lazy<ConcurrentBag<MemoryArray>>();
            }
        }

        public MemoryArray RentMemoryArray(int capacity)
        {
            var size = getSize(capacity);
            var index = getIndex(size);

            var bag = pool[index].Value;
            if(bag.TryTake(out var res))
            {
                Interlocked.Decrement(ref poolSizes[index]);
                return res;
            }
            MemoryArray mem = new MemoryArray(size, ALIGN_MEMORY);
            return mem;
        }
        public void ReturnMemoryArray(MemoryArray memoryArray)
        {
            var index = getIndex((uint)memoryArray.TotalSize);

            var bag = pool[index].Value;

            if(Volatile.Read(ref poolSizes[index]) >= MAX_BAG_ELEMENT_COUNT)
            {
                memoryArray.Dispose();
                return;
            }
            bag.Add(memoryArray);
            Interlocked.Increment(ref poolSizes[index]);            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint getSize(int requiredSize)
        {
            return roundUpP2(Math.Max((uint)requiredSize, MINIMAL_MEMORY_ARRAY_SIZE));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint roundUpP2(uint val)
        {
#if NET6_0_OR_GREATER
            return BitOperations.RoundUpToPowerOf2(val);
#else
            val--;            
            val |= val >> 1;
            val |= val >> 2;
            val |= val >> 4;
            val |= val >> 8;
            val |= val >> 16;
            val++;
            return val;
#endif
        }
        private static int getIndex(uint val)
        {
#if NET6_0_OR_GREATER
            return BitOperations.TrailingZeroCount(val);
#else
            if(val==0) return 0;

            int t = 1;
            int r = 0;
            while((val & t) == 0){
                t = t << 1;
                r++;
            }
            return r;
#endif
        }

        public void Dispose()
        {
            foreach(var item in pool)
            {
                foreach(var arr in item.Value)
                {
                    arr.Dispose();
                }
            }
        }
    }
}
