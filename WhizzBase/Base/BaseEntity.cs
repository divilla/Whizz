using System.Collections.Generic;

namespace WhizzBase.Base
{
    public abstract class BaseEntity
    {
        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
        private readonly HashSet<string> dirtyProperties = new HashSet<string>();

        public void MarkDirty(string propertyName)
        {
            if (dirtyProperties.Contains(propertyName)) return;
            dirtyProperties.Add(propertyName);
        }
    }
}