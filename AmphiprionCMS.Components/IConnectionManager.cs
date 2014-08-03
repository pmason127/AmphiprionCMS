using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmphiprionCMS.Components;

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
            SqlConnection con = null;
            if(AmphiprionCMSInitializer.Configuration.ConnectionString != null)
                con = new SqlConnection(AmphiprionCMSInitializer.Configuration.ConnectionString);

            if (con == null && AmphiprionCMSInitializer.Configuration.ConnectionStringName != null)
            {
                var connctionString = System.Configuration.ConfigurationManager.ConnectionStrings[AmphiprionCMSInitializer.Configuration.ConnectionStringName].ConnectionString;
                 con = new SqlConnection(connctionString);
            }

            if(con == null)
                throw new SiteNotConfiguredException();

            if(openConnection)
                con.Open();

            return con;
        }
    }
}
