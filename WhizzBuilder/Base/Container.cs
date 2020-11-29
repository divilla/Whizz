using System.Collections.Generic;
using System.Linq;

namespace WhizzBuilder.Base
{
    public abstract class Container
    {
        protected virtual int Indents => 0;
        protected string Indet = "    ";
        protected string Indentation
        {
            get
            {
                var result = "";
                for (var i = 0; i < Indents; i++)
                {
                    result += Indet;
                }

                return result;
            }
        }

        protected readonly List<Element> Elements = new List<Element>();

        public virtual string Build()
        {
            return Elements.Aggregate("", (current, element) => current + element.Build());
        }
    }
}