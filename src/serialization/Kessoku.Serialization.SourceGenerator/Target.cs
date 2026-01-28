using System;
using System.Collections.Generic;
using System.Text;

namespace Kessoku.Serialization.SourceGenerator
{
    public record Target
    {
        public string Namespace { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;

        /// <summary>
        /// Is base type also implements ISerializable and do we need to include his output
        /// </summary>
        public bool IsBaseTypeAlsoTarget { get; set; } = false;

        /// <summary>
        /// Class or stucture
        /// </summary>
        public string Type { get; set;  } = string.Empty;

        public List<Member> Members { get; set; } = new List<Member>();
    }

    /// <summary>
    /// Represent single member(property/struct) in type
    /// </summary>
    public record Member {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
