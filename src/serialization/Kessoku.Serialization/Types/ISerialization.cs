using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kessoku.Serialization.Types
{
    public interface ISerialization
    {
        void Serialize(BinaryWriter binaryWriter);
        void Deserialize(BinaryReader binaryReader);
    }
}
