using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Kessoku.Serialization.Types
{
    /// <summary>
    /// Base type for all class messages or smth.
    /// </summary>
    public abstract class DTO : IDisposable, ISerialization
    {
        /// <summary>
        /// Return DTO to his own pool.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Method to serialize this object.
        /// </summary>
        /// <typeparam name="TBufferWriter"></typeparam>
        /// <param name="binaryWriter"></param>
        public abstract void Serialize<TBufferWriter>(TBufferWriter binaryWriter) where TBufferWriter : IBufferWriter<byte>;

        public virtual uint GetSize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Message id.
        /// </summary>
        public abstract int Id { get; }
    }
}
