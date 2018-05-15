using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CoreMapper
{
    public class MapperMember
    {

        public MapperMember(string name,Type type,MemberInfo memberInfo)
        {
            Name = name;
            Type = type;
            MemberInfo = memberInfo;
            IsPrimitive = Helper.IsPrimitive(type);
        }

        public string Name { get; }
        public Type Type { get; }
        public MemberInfo MemberInfo { get; }
        public bool IsPrimitive { get; }
    }
}
