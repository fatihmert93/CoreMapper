using System;
using System.Collections.Generic;

namespace CoreMapper
{
    public class MapConfiguration
    {
        private readonly Dictionary<int, IMapDefinition> _mapDefinitions = new Dictionary<int, IMapDefinition>();
        


        public MapConfiguration()
        {
        }
    }
}
