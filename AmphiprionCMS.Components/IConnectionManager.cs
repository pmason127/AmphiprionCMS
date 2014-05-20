using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphiprion.Data
{
    public interface  IConnectionManager
    {
        IDbConnection GetConnection(bool openConnection = true,dynamic options = null);
    }

    public class MSSQLConnectionManager : IConnectionManager
    {

        public IDbConnection GetConnection(bool openConnection = true, dynamic options = null)
        {
            var cStr = "amp";
            if (options != null && options.ConnectionStringName != null)
                cStr = options.ConnectionStringName;

            var connctionString = System.Configuration.ConfigurationManager.ConnectionStrings[cStr].ConnectionString;
            var con = new SqlConnection(connctionString);

            if(openConnection)
                con.Open();

            return con;
        }
    }
}
