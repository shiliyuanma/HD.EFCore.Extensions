using System;

namespace HD.EFCore.Extensions.Cache
{
    public class EntityCacheOptions
    {
        public Func<string, object> Get;

        public Func<string, object, bool> Set;

        public Func<string, bool> Del;

        public Func<MapItem, object> Map;
    }

    public class MapItem
    {
        public Type EntityType { get; set; }

        public object EntityVal { get; set; }
    }
}
