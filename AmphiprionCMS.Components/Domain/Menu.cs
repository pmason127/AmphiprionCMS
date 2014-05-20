using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amphiprion.Data.Entities
{
   public class Menu
    {
       public virtual int Id { get; set; }
       public virtual string Name { get; set; }
       public virtual IList<MenuItem> MenuItems { get; set; } 
    }

    public class MenuItem
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Key { get; set; }
        public virtual IList<MenuItem> MenuItems { get; set; } 
    }
}
