using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsXUnitTests
{
    public class TestBase
    {
        protected IConfiguration Configuration { get; }

        public TestBase()
        {
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:Default", "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GuildsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" }
                })
                .Build();
        }
    }
}
