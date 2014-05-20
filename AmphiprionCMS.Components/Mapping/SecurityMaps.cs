using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmphiprionCMS.Components.Security;
using DapperExtensions.Mapper;
using Microsoft.AspNet.Identity;

namespace AmphiprionCMS.Components.Mapping
{
    public class UserMap : ClassMapper<CMSUser>
    {
        public UserMap()
        {
           Table("ampUser");
            Map(u => u.Id).Key(KeyType.Assigned);
            Map(u => u.Roles).Ignore();
         //   Map(u => u.IsAnonymous).Ignore();
            AutoMap();
        }
    }
    public class RoleMap : ClassMapper<CMSRole>
    {
        public RoleMap()
        {
            Table("ampRole");
            Map(u => u.Id).Key(KeyType.Assigned);
            Map(u => u.Name).Ignore();
            AutoMap();
        }
    }
}
