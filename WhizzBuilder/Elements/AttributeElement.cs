using System.ComponentModel.DataAnnotations;
using System.Linq;
using WhizzBuilder.Base;

namespace WhizzBuilder.Elements
{
    public class AttributeElement : Element
    {
        public AttributeElement(string name, params object[] values)
        {
            Name = name;
            Values = values;
        }

        private string _newLine = Application.NewLineChar;
        
        [Required] private string Name { get; }
        private object[] Values { get; }

        public AttributeElement NoNewLine()
        {
            _newLine = "";
            return this;
        }

        public override string Build()
        {
            return Values.Length == 0 ? $"[{Name}]{_newLine}" : $"[{Name}({BuildValues()})]{_newLine}";
        }

        private string BuildValues()
        {
            var values = Values.Select(value => value is string ? $"\"{value}\"" : value.ToString()).ToList();

            return string.Join(", ", values);
        }
    }
}