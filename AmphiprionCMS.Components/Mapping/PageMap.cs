using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amphiprion.Data.Entities;
using DapperExtensions.Mapper;

namespace Amphiprion.Data.Mapping
{
    public class PageMap:ClassMapper<Page>
    {
        public PageMap()
        {
           Table("ampPage");
           Map(p => p.Id).Key(KeyType.Assigned);
            Map(p => p.AccessDefinition).Ignore();
            Map(p => p.IsPagePublished).Ignore();
           AutoMap();
        }
    }

    public class AccessDefinitionMap : ClassMapper<AccessDefinition>
    {
        public AccessDefinitionMap()
        {
            Table("ampAccessDefinition");
            Map(u => u.PageId).Key(KeyType.Assigned);
            Map(u => u.RoleId).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
