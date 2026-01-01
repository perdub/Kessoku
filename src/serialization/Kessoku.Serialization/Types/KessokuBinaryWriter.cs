using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kessoku.Serialization.Types
{
    public class KessokuBinaryWriter : BinaryWriter
    {
        public KessokuBinaryWriter(Stream s) : base(s)
        {

        }

        public KessokuBinaryWriter(Stream output, Encoding encoding) : base(output, encoding) { }

        public KessokuBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }

        /// <summary>
        /// Check string for null.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(string value)
        {
#pragma warning disable
            if (value == null)
            {
                value = String.Empty;
            }
#pragma warning restore
            base.Write(value);
        }

        /// <summary>
        /// Additional array length writing.
        /// </summary>
        /// <param name="buffer"></param>
        public override void Write(byte[] buffer)
        {
            this.Write(buffer, false);
        }

        public void Write(byte[] buffer, bool skipWriteLength)
        {
            if (!skipWriteLength)
            {
                Write(buffer.Length);
            }
            base.Write(buffer);
        }

        public void Write(Guid guid)
        {
            base.Write(guid.ToByteArray());
        }
    }
}
