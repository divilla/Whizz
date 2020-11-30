using System;

namespace WhizzBase.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewAttribute : BaseAttribute
    {
        public ViewAttribute(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}
