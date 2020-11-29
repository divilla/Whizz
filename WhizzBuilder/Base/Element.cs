using System.ComponentModel.DataAnnotations;

namespace WhizzBuilder.Base
{
    public abstract class Element : Container
    {
        [Required] protected string _name;
    }
}