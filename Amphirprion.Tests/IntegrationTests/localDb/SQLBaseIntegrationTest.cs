using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data;
using Amphiprion.Data.Entities;

using Amphiprion.Tests;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.SQL;
using NUnit.Framework;

namespace Amphirprion.Tests.IntegrationTests.localDb
{
    public class LocalDBConnectionManager:IConnectionManager 
    {

        public System.Data.IDbConnection GetConnection(bool openConnection = true, dynamic options = null)
        {
            return LocalDB.GetLocalDB("amp_tests");
        }
    }
     [TestFixture]
    public abstract class SQLBaseIntegrationTest
    {
         [TestFixtureSetUp]
         public virtual void Setup()
         {
             IInstaller inst = new MSSQLInstaller(this.ConnectionManager);
             inst.Install();

             BeforeTests();
         }

         public virtual void BeforeTests()
         {
             
         }
        public SQLBaseIntegrationTest()
        {
            ConnectionManager = new LocalDBConnectionManager();
            Repository = new PageRepository (ConnectionManager);
            UserRepository = new CMSUserRepository(ConnectionManager);
        }
        public IConnectionManager ConnectionManager { get; set; }
        public IPageRepository Repository { get; set; }
         public ICMSUserRepository UserRepository
         {
             get; set;
         }
    }
}
