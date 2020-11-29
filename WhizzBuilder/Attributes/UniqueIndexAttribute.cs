using System;

namespace WhizzBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueIndexAttribute : BaseAttribute
    {
        public UniqueIndexAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
