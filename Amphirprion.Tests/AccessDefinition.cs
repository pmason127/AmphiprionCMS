using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data.Entities;
using NUnit.Framework;

namespace Amphirprion.Tests
{
    [TestFixture]
    public class AccessDefinitionTests
    {
        [Test]
        public void CanGetSingleDefinitionFromList()
        {
            var roles = new string[] {"everyone","users","publishers"};

            var definitions = new List<AccessDefinition>()
            {
                new AccessDefinition(){RoleId ="administrator",CanRead =false,CanEdit = true,CanDelete = false,CanPublish = true},
                 new AccessDefinition(){RoleId ="users",CanRead =false,CanEdit = false,CanDelete = true,CanPublish = false},
                  new AccessDefinition(){RoleId ="everyone",CanRead =true,CanEdit = false,CanDelete = false,CanPublish = false}
            };

            var emptyDef = roles.Select(r => new AccessDefinition() {RoleId = r});

            var ins = definitions.Intersect(emptyDef,new AccessDefinitionComparer());

            var normalizedView = new AccessDefinition();
            foreach (var ad in ins)
            {
                if (ad.CanRead)
                    normalizedView.CanRead = true;
                if (ad.CanPublish)
                    normalizedView.CanPublish = true;
                if (ad.CanEdit)
                    normalizedView.CanEdit = true;
                if (ad.CanDelete)
                    normalizedView.CanDelete = true;
            }

            Assert.IsTrue(normalizedView.CanRead && normalizedView.CanDelete && !normalizedView.CanEdit && !normalizedView.CanPublish);
        }
    }
}
