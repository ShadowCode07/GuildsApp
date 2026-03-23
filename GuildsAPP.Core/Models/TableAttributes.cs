using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsApp.Core.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; }
        public string PrimaryKey { get; }

        public TableAttribute(string name, string primaryKey = "Id")
        {
            Name = name;
            PrimaryKey = primaryKey;
        }
    }
}
    