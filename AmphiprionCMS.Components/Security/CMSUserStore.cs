using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AmphiprionCMS.Components.SQL;
using Microsoft.AspNet.Identity;

namespace AmphiprionCMS.Components.Security
{
    public class CMSUserStore:IUserStore<CMSUser,Guid>
        ,IUserPasswordStore<CMSUser,Guid>
        ,IUserEmailStore<CMSUser,Guid>
        ,IRoleStore<CMSRole,string>
        ,IUserRoleStore<CMSUser,Guid>
        ,IQueryableRoleStore<CMSRole,string>
    {
        private ICMSUserRepository _repo;
        public CMSUserStore(ICMSUserRepository repo)
        {
            _repo = repo;
        }
        #region IUserStore
        public Task CreateAsync(CMSUser user)
        {
            _repo.CreateUser(user);
            _repo.AddUserToRole(user.Id,"everyone");
            _repo.AddUserToRole(user.Id, "users");
            return Task.FromResult(0);
        }

        public Task DeleteAsync(CMSUser user)
        {
            bool deleted = _repo.DeleteUser(user.Id,false);
            return Task.FromResult(deleted);
        }

        public Task<CMSUser> FindByIdAsync(Guid userId)
        {
            var user = _repo.GetUser(userId);
            AddRolesToUser(user);
            return Task.FromResult(user);
        }

        private void AddRolesToUser(CMSUser user)
        {
            if (user == null) return;
            var roles = _repo.GetRoles(user.Id);
            user.Roles = roles.Select(r => r.Id).ToArray();
        }
        public Task<CMSUser> FindByNameAsync(string userName)
        {
            var user = _repo.GetUser(userName);
            AddRolesToUser(user);
            return Task.FromResult(user);
        }

        public Task UpdateAsync(CMSUser user)
        {
            _repo.UpdateUser(user);
            return Task.FromResult(0);
        }
        #endregion

        public void Dispose()
        {
            
        }
        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(CMSUser user)
        {
            var hash = _repo.GetPasswordHash(user.Id);
            return Task.FromResult(hash);
        }

        public Task<bool> HasPasswordAsync(CMSUser user)
        {
            var hash = _repo.GetPasswordHash(user.Id);
            return Task.FromResult(!string.IsNullOrEmpty(hash));
        }

        public Task SetPasswordHashAsync(CMSUser user, string passwordHash)
        {
           bool ok = _repo.SetPasswordHash(user.Id, passwordHash);
           return Task.FromResult(0);
        }
        #endregion
        #region IUserEmailStore
        public Task<CMSUser> FindByEmailAsync(string email)
        {
            var user = _repo.GetUserByEmail(email);
            AddRolesToUser(user);
            return Task.FromResult(user);
        }

        public Task<string> GetEmailAsync(CMSUser user)
        {
            var usr = _repo.GetUser(user.Id);
            return Task.FromResult(usr.Email);
        }

        public Task SetEmailAsync(CMSUser user, string email)
        {
            if(!string.IsNullOrEmpty(user.Email) && user.Email.Equals(email,StringComparison.InvariantCultureIgnoreCase))
                 return Task.FromResult(0);

            var usr = _repo.GetUser(user.Id);
            usr.Email = email;
            _repo.UpdateUser(usr);
            return Task.FromResult(0);
        }
        #endregion

        public Task CreateAsync(CMSRole role)
        {
            if(string.IsNullOrEmpty(role.Name))
                throw new ArgumentException("Role name is required","Name");

            if (string.IsNullOrEmpty(role.Id))
                role.Id = ToId(role.Name);
          
            _repo.CreateRole(role);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(CMSRole role)
        {
            _repo.DeleteRole(role.Id);
            return Task.FromResult(0);
        }

        public Task<CMSRole> FindByIdAsync(string roleId)
        {
            var role = _repo.GetRoleById(roleId);
            return Task.FromResult(role);
        }

        Task<CMSRole> IRoleStore<CMSRole, string>.FindByNameAsync(string roleName)
        {
            var role = _repo.GetRoleById(ToId(roleName));
            return Task.FromResult(role);
        }

        public Task UpdateAsync(CMSRole role)
        {
            throw new NotImplementedException();
        }

        private string ToId(string text)
        {
           var str = Regex.Replace(text, @"[^a-z0-9\s-]", "");
            return str.ToLower();
        }

        public Task AddToRoleAsync(CMSUser user, string roleName)
        {
            if (!_repo.IsUserInRole(user.Id, roleName))
            {
                _repo.AddUserToRole(user.Id,roleName);
            }
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(CMSUser user)
        {
            var roles = _repo.GetRoles(user.Id);
            IList<string> roleStrings = roles.Select(r => r.Id).ToList();
            return Task.FromResult(roleStrings);
        }

        public Task<bool> IsInRoleAsync(CMSUser user, string roleName)
        {
            bool inRole = _repo.IsUserInRole(user.Id, roleName);
            return Task.FromResult(inRole);
        }

        public Task RemoveFromRoleAsync(CMSUser user, string roleName)
        {
            if (_repo.IsUserInRole(user.Id, roleName))
            {
                _repo.RemoveUserFromRole(user.Id, roleName);
            }
            return Task.FromResult(0);
        }

        public IQueryable<CMSRole> Roles
        {
            get
            {
                return new EnumerableQuery<CMSRole>(_repo.GetRoles());
            }
        }
    }
}
