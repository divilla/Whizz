using System;

namespace WhizzBase.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : BaseAttribute
    {
        public TableAttribute(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}
