using System;
using WhizzBuilder.Base;

namespace WhizzBuilder.Elements
{
    public class NamespaceElement : Element
    {
        public NamespaceElement Name(string value)
        {
            _name = value;
            return this;
        }

        public NamespaceElement AddClass(Action<ClassElement> action)
        {
            var classElement = new ClassElement();
            action(classElement);
            Elements.Add(classElement);

            return this;
        }

        public override string Build()
        {
            return @$"
{Indentation}namespace {_name}
{Indentation}{{{base.Build()}
{Indentation}}}";
        }
    }
}