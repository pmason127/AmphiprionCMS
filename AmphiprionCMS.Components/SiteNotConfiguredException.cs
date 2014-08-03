using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmphiprionCMS.Components
{
    public class SiteNotConfiguredException:Exception
    {
        public SiteNotConfiguredException():base("This site has not been configured")
        {
                
        }
    }
}
