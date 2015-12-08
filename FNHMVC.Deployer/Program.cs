using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNHMVC.Deployer
{
    class Program
    {
        static void Main(string[] args)
        {
            ///FNHMVC.Data.SchemaTool.SchemaTool.UpdateSchema(FNHMVC.Data.Infrastructure.Utility.ConnectionStringName);
            FNHMVC.Data.SchemaTool.SchemaTool.CreatSchema("", FNHMVC.Data.Infrastructure.Utility.ConnectionStringName);
        }
    }
}
