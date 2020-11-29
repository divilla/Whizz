using System;
using System.ComponentModel.DataAnnotations;
using WhizzBuilder.Elements;

namespace WhizzBuilder.Base
{
    public class File : Container
    {
        [Required] private string _targetFolder;
        [Required] private string _filename;

        public File SetTargetFolder(string value)
        {
            _targetFolder = value;
            return this;
        }
        
        public File SetFilename(string value)
        {
            _filename = value;
            return this;
        }
        
        public File AddUsing(string value)
        {
            Elements.Add(new UsingElement(value));
            return this;
        }

        public File AddNamespace(Action<NamespaceElement> namespaceAction)
        {
            var nsp = new NamespaceElement();
            namespaceAction(nsp);
            Elements.Add(nsp);
            return this;
        }
    }
}