using System;
using System.Collections.Generic;
using System.Text;

namespace Kessoku.Serialization.Types
{
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class IgnoreMemberAttribute : Attribute
    {
        public IgnoreMemberAttribute()
        {
        }

    }
}
