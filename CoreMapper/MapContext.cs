using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreMapper
{
    public sealed class MapContext
    {
        internal static readonly MethodInfo NewInstanceMethod;
        internal static readonly MethodInfo GetFromCacheMethod;

        private readonly Dictionary<int, object> _referenceCache = new Dictionary<int, object>();
        private readonly MapConfiguration _mapper;

        public MapContext()
        {
            var type = typeof(MapContext);
            var methods = type.GetRuntimeMethods().ToList();
        }

    }
}
