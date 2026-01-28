using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kessoku.Serialization.Types
{
    /// <summary>
    /// All objects, which needed to be serialized/deserialized, should implement this interface. Mostly, source generator generate implementation automatic.
    /// </summary>
    public interface ISerialization
    {
        void Serialize<TBufferWriter>(TBufferWriter binaryWriter) where TBufferWriter : IBufferWriter<byte>;       
    }
}
