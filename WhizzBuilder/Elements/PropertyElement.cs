using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WhizzBuilder.Base;
using WhizzBuilder.Constants;
using WhizzBase.Extensions;

namespace WhizzBuilder.Elements
{
    public class PropertyElement : Element
    {
        protected override int Indents => 2;

        private AttributeElement _primaryKeyAttribute;
        private AttributeElement _compositePrimaryKeyAttribute;
        private AttributeElement _foreignKeyAttribute;
        private List<AttributeElement> _uniqueIndexAttributes = new List<AttributeElement>();
        private AttributeElement _columnAttribute;
        [Required] private string _type;
        private string _accessModifier = "public ";
        private string _memberModifiers;
        private bool _markDirty;

        public PropertyElement AddPrimaryKeyAttribute()
        {
            _primaryKeyAttribute = new AttributeElement("PrimaryKey");
            return this;
        }

        public PropertyElement AddCompositePrimaryKeyAttribute()
        {
            _compositePrimaryKeyAttribute = new AttributeElement("CompositePrimaryKey");
            return this;
        }

        public PropertyElement AddForeignKeyAttribute(string relationName, string columnName)
        {
            _foreignKeyAttribute = new AttributeElement("ForeignKey", relationName, columnName);
            return this;
        }

        public PropertyElement AddUniqueIndexAttribute(string indexName)
        {
            _uniqueIndexAttributes.Add(new AttributeElement("UniqueIndex", indexName));
            return this;
        }

        public PropertyElement AddColumnAttribute(string name, short position)
        {
            _columnAttribute = new AttributeElement("Column", name, position);
            return this;
        }

        public PropertyElement Name(string value)
        {
            _name = value;
            return this;
        }

        public PropertyElement Type(string value)
        {
            _type = value;
            return this;
        }

        public PropertyElement AccessModifier(Func<AccessModifier, string> accessModifierFunc)
        {
            var accessModifier = new AccessModifier();
            _accessModifier = accessModifierFunc(accessModifier) + " ";

            return this;
        }

        public PropertyElement MemberModifiers(params Func<MemberModifier, string>[] memberModifierFuncs)
        {
            _memberModifiers = "";
            var memberModifier = new MemberModifier();
            foreach (var memberModifierFunc in memberModifierFuncs)
            {
                _memberModifiers += memberModifierFunc(memberModifier) + " ";
            }

            return this;
        }

        public PropertyElement MarkDirty()
        {
            _markDirty = true;
            return this;
        }

        public override string Build()
        {
            if (!_markDirty)
                return @$"
{BuildAttributes()}{Indentation}{_accessModifier}{_memberModifiers}{_type} {_name} {{ get; set; }}
";
            
            var privateVar = _name.ToUnderscoredCamelCase();
            return $@"
{BuildAttributes()}{Indentation}{_accessModifier}{_memberModifiers}{_type} {_name}
{Indentation}{{
{Indentation}{Indet}get => {privateVar};
{Indentation}{Indet}set
{Indentation}{Indet}{{
{Indentation}{Indet}{Indet}{privateVar} = value;
{Indentation}{Indet}{Indet}MarkDirty(""{_name}"");
{Indentation}{Indet}}}
{Indentation}}}
{Indentation}private {_type} {privateVar};
";
        }

        private string BuildAttributes()
        {
            var attributes = "";
            attributes += _primaryKeyAttribute == null ? "" : Indentation + _primaryKeyAttribute.Build();
            attributes += _compositePrimaryKeyAttribute == null ? "" : Indentation + _compositePrimaryKeyAttribute.Build();
            attributes += _foreignKeyAttribute == null ? "" : Indentation + _foreignKeyAttribute.Build();
            foreach (var attribute in _uniqueIndexAttributes)
            {
                attributes += Indentation + attribute.Build();
            }
            attributes += _columnAttribute == null ? "" : Indentation + _columnAttribute.Build();

            return attributes;
        }
    }
}
