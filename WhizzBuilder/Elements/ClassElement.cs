using System;
using WhizzBuilder.Base;
using WhizzBuilder.Constants;

namespace WhizzBuilder.Elements
{
    public class ClassElement : Element
    {
        protected override int Indents => 1;

        private AttributeElement _tableViewAttribute;
        private string _accessModifier = "public ";
        private string _classModifiers = "";
        private string _parentClass = "";

        public ClassElement AddTableAttribute(string value)
        {
            _tableViewAttribute = new AttributeElement("Table", value);
            return this;
        }

        public ClassElement AddViewAttribute(string value)
        {
            _tableViewAttribute = new AttributeElement("View", value);
            return this;
        }

        public ClassElement Name(string value)
        {
            _name = value;
            return this;
        }

        public ClassElement AccessModifier(Func<AccessModifier, string> accessModifierFunc)
        {
            var accessModifier = new AccessModifier();
            _accessModifier = accessModifierFunc(accessModifier) + " ";

            return this;
        }

        public ClassElement ClassModifiers(params Func<ClassModifier, string>[] classModifierFuncs)
        {
            _classModifiers = "";
            var classModifier = new ClassModifier();
            foreach (var classModifierFunc in classModifierFuncs)
            {
                _classModifiers += classModifierFunc(classModifier) + " ";
            }

            return this;
        }

        public ClassElement AddBaseClass(string name)
        {
            _parentClass = $" : {name}";

            return this;
        }

        public ClassElement AddProperty(Action<PropertyElement> propertyAction)
        {
            var element = new PropertyElement();
            propertyAction(element);
            Elements.Add(element);
            
            return this;
        }
        
        public override string Build()
        {
            return _tableViewAttribute != null ? @$"
{Indentation}{_tableViewAttribute.NoNewLine().Build()}
{Indentation}{_accessModifier}{_classModifiers}class {_name}{_parentClass}
{Indentation}{{{base.Build()}{Indentation}}}
" : @$"
{Indentation}{_accessModifier}{_classModifiers}class {_name}
{Indentation}{{{base.Build()}{Indentation}}}
";
        }
    }
}