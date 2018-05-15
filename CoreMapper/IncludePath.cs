using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreMapper
{
    public sealed class IncludePath
    {

        public IncludePath(string member): this(member, Enumerable.Empty<IncludePath>())
        {
        }

        public IncludePath(string member,IEnumerable<IncludePath> children)
        {
            Member = member;
            Children = children;
        }
        public string Member { get; }

        public IEnumerable<IncludePath> Children { get; }
    }
}
