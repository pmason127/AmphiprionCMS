using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace Amphiprion.Data
{
    public interface IInstaller
    {
        void Install();
        string GetInstallScript();
    }

    public class InstallerUtility
    {
         public static string ReadScriptFile(string name, string provider)
        {
            string fileName =  "AmphiprionCMS.Components." + provider +".scripts." + name + ".sql";
            using (Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
            using (StreamReader sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }
    }

    public class MSSQLInstaller : IInstaller
    {
        private IConnectionManager _connectionManager;
        public MSSQLInstaller(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public string GetInstallScript()
        {
            var sql = InstallerUtility.ReadScriptFile("base", "SQL");
            return sql;
        }
        public void Install()
        {
            var files = new List<string>
                                {
                                    InstallerUtility.ReadScriptFile("base","SQL"),
                                };

            using (var c = _connectionManager.GetConnection())
            {
                var t = c.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var str in files)
                    {
                        c.Execute(str, null, t);
                    }
                    t.Commit();
                }
                catch (Exception)
                {
                    t.Rollback();
                    throw;
                }
               
                c.Close();
            }
           

        }
    }
}
