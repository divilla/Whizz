using System;

namespace WhizzBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : BaseAttribute
    {
        public ColumnAttribute(string name, ushort number)
        {
            Name = name;
            Number = number;
        }
        
        public string Name { get; }
        public ushort Number { get; }
    }
}
