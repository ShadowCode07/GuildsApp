using System.ComponentModel.DataAnnotations.Schema;

namespace GuidlsMVC.Entities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; }

        public  TableAttribute(string name)
        {
            Name = name;
        }
    }
}
