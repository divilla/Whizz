using System;

namespace WhizzBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Length : BaseAttribute
    {
        public Length(int max, int min = 0)
        {
            Min = min;
            Max = max;
        }

        public int Min { get; }
        public int Max { get; }
    }
}
