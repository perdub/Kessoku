// this type is so shit.


using Kessoku.Serialization.Exceptions;
using Kessoku.Serialization.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
namespace Kessoku.Serialization.Helpers
{
    /// <summary>
    /// This type uses by serialization to get info about needed memory.
    /// In general cases, we don`t check elements for null in this type.
    /// </summary>
    public ref struct SizeEstimate
    {
        public SizeEstimate()
        {
            Size = 0;
        }

        public uint Size { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddField(string? str)
        {
            if (str == null)
            {
                Size++;
                return;
            }

            Size += 2 + (uint)Encoding.UTF8.GetByteCount(str);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddField<T>(T[]? array)
        {
#if NET5_0_OR_GREATER
            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                Size += 4;
                if(array == null)
                {
                    return;
                }
                Size += (uint)array.Length * (uint)Unsafe.SizeOf<T>();
                return;
            }
#endif
            if(array is not null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    AddField(array[i]);
                }
            }
            Size += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddField<T>(IEnumerable<T>? collection)
        {
            Size += 4;

            if (collection == null)
                return;

            foreach (var item in collection)
                AddField(item);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddField<T>(ISerialization value)
        {
            Size += value.GetSize();
        }

        /// <summary>
        /// Estimate size for arbitraty type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <exception cref="EstimateSizeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddField<T>(T value)
        {
#if NET5_0_OR_GREATER
            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                Size += (uint)Unsafe.SizeOf<T>();
                return;
            }
#endif

            throw new EstimateSizeException($"{typeof(T).FullName} can`t be estimated.");
        }

    }
}
