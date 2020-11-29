using System.ComponentModel.DataAnnotations;
using WhizzBuilder.Base;

namespace WhizzBuilder.Elements
{
    public class UsingElement : Element
    {
        public UsingElement(string value)
        {
            Value = value;
        }

        [Required] private string Value { get; }

        public override string Build()
        {
            return $"using {Value};\r\n";
        }
    }
}