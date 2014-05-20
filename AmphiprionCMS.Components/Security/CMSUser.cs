using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AmphiprionCMS.Components.Security
{
    public class CMSUser:IUser<Guid>
    {
        public CMSUser()
        {
            IsActive = true;
            Roles = new string[]{};
        }
        public Guid Id { get;  set; }
        public string UserName { get;  set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public bool IsActive { get; set; }
       // public bool IsAnonymous { get { return string.IsNullOrEmpty(UserName) || UserName == "anonymous"; } }
    }

    public class CMSRole:IRole<string>
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get { return Id; } set { Id = value; } }
    }
}
