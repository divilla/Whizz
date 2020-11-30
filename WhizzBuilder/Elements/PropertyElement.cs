using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WhizzBuilder.Base;
using WhizzBuilder.Constants;
using WhizzBase.Extensions;

namespace WhizzBuilder.Elements
{
    public class PropertyElement : Element
    {
        protected override int Indents => 2;

        private List<AttributeElement> _attributes = new List<AttributeElement>();
        [Required] private string _type;
        private string _accessModifier = "public ";
        private string _memberModifiers;
        private bool _markDirty;

        public PropertyElement AddPrimaryKeyAttribute()
        {
            _attributes.Add(new AttributeElement("PrimaryKey"));
            return this;
        }

        public PropertyElement AddCompositePrimaryKeyAttribute()
        {
            _attributes.Add(new AttributeElement("CompositePrimaryKey"));
            return this;
        }

        public PropertyElement AddRequiredAttribute()
        {
            _attributes.Add(new AttributeElement("Required"));
            return this;
        }

        public PropertyElement AddReadonlyAttribute()
        {
            _attributes.Add(new AttributeElement("Readonly"));
            return this;
        }

        public PropertyElement AddLengthAttribute(int max)
        {
            _attributes.Add(new AttributeElement("Length", max));
            return this;
        }

        public PropertyElement AddForeignKeyAttribute(string relationName, string columnName)
        {
            _attributes.Add(new AttributeElement("ForeignKey", relationName, columnName));
            return this;
        }

        public PropertyElement AddUniqueIndexAttribute(string indexName)
        {
            _attributes.Add(new AttributeElement("UniqueIndex", indexName));
            return this;
        }

        public PropertyElement AddColumnAttribute(string name, short position)
        {
            _attributes.Add(new AttributeElement("Column", name, position));
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
            return _attributes.Aggregate("", (current, attribute) => current + (Indentation + attribute.Build()));
        }
    }
}
