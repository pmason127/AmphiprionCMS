using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DapperExtensions.Mapper;

namespace AmphiprionCMS.Components.Mapping
{
    public class SettingsMap : ClassMapper<Amphiprion.Data.Entities.SiteSettings>
    {
        public SettingsMap()
        {
            Table("ampSettings");
            Map(p => p.Id).Column("SiteId").Key(KeyType.Assigned);
            Map(p => p.TimeZoneInfo).Ignore();
                AutoMap();
        }
    }
}
