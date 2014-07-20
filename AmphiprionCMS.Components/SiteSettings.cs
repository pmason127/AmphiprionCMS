using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmphiprionCMS.Components.SQL;
using Microsoft.Practices.ServiceLocation;

namespace AmphiprionCMS.Components
{
   public  class SiteSettings
   {
       private static Amphiprion.Data.Entities.SiteSettings _settings = null;
       private static readonly int _defaultSiteSettingsId = 1;
       private static readonly object _lock = new object();
       private static ISettingsRepository _repo = ServiceLocator.Current.GetInstance<ISettingsRepository>();
       static  SiteSettings()
       {
            
       }
       public static  Amphiprion.Data.Entities.SiteSettings Current
       {
           get
           {
               lock (_lock)
               {
                   if (_settings == null)
                   {
                       _settings = _repo.Get(_defaultSiteSettingsId);
                   }
               }
              
               return _settings;
           }
       }

       public static void Save()
       {
           lock (_lock)
           {
               if (_settings == null)
                   return;
               _repo.Update(_settings);
               _settings = _repo.Get(_defaultSiteSettingsId);
           }
         
       }
   }
}
