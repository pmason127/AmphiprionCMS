using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data;
using AmphiprionCMS.Components.Security;
using Dapper;
using DapperExtensions;

namespace AmphiprionCMS.Components.SQL
{
    public interface ICMSUserRepository
    {
        void CreateUser(CMSUser user);
        void UpdateUser(CMSUser user);
        CMSUser GetUser(Guid id);
        CMSUser GetUser(string userName);
        CMSUser GetUserByEmail(string email);
        bool DeleteUser(Guid id, bool permanent = false);
        string GetPasswordHash(Guid userId);
        bool SetPasswordHash(Guid userId, string hash);
        void CreateRole(CMSRole role);
        void DeleteRole(string id);
        CMSRole GetRoleById(string id);
        IList<CMSRole> GetRoles();
        IList<CMSRole> GetRoles(Guid userId);
        void AddUserToRole(Guid userId, string role);
        void RemoveUserFromRole(Guid userId, string role);
        bool IsUserInRole(Guid userId, string roleId);
    }

    public class CMSUserRepository : ICMSUserRepository
    {
        private IConnectionManager _connectionManager;
        public CMSUserRepository(IConnectionManager manager)
        {
            _connectionManager = manager;
        }

        public void CreateUser(CMSUser user)
        {
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            using (var c = _connectionManager.GetConnection())
            {
                using (var t = c.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        c.Insert<CMSUser>(user,t);
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                    t.Commit();
                }
                c.Close();
            }
        }
        public void UpdateUser(CMSUser user)
        {
            using (var c = _connectionManager.GetConnection())
            {
                using (var t = c.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        c.Update<CMSUser>(user,t);
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                    t.Commit();
                }
                c.Close();
            }
        }
        public CMSUser GetUser(Guid id)
        {
            CMSUser user = null;
            using (var c = _connectionManager.GetConnection())
            {
                var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                pg.Predicates.Add(Predicates.Field<CMSUser>(u => u.Id, Operator.Eq, id));
                pg.Predicates.Add(Predicates.Field<CMSUser>(u => u.IsActive, Operator.Eq, true));
                user = c.GetList<CMSUser>(pg).FirstOrDefault();
                c.Close();
            }
            return user;
        }
        public CMSUser GetUser(string userName)
        {
            CMSUser user = null;
            using (var c = _connectionManager.GetConnection())
            {
                var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                 pg.Predicates.Add(Predicates.Field<CMSUser>(u => u.UserName, Operator.Eq, userName));
                 pg.Predicates.Add(Predicates.Field<CMSUser>(u => u.IsActive, Operator.Eq, true));
                user = c.GetList<CMSUser>(pg).FirstOrDefault();

                c.Close();
            }
            return user;
        }
        public CMSUser GetUserByEmail(string email)
        {
            CMSUser user = null;
            using (var c = _connectionManager.GetConnection())
            {
                var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                pg.Predicates.Add(Predicates.Field<CMSUser>(u => u.Email, Operator.Eq, email));
                pg.Predicates.Add(Predicates.Field<CMSUser>(u => u.IsActive, Operator.Eq, true));
                user = c.GetList<CMSUser>(pg).FirstOrDefault();
                c.Close();
            }
            return user;
        }

        public bool DeleteUser(Guid id, bool permanent = false)
        {
            CMSUser user = null;
            using (var c = _connectionManager.GetConnection())
            {
                using (var t = c.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        if (permanent)
                            c.Delete<CMSUser>(id, t);
                        else
                        {
                            c.Execute("update ampUser set IsActive = 0 where Id=@id", new { id = id }, t);
                        }
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                    t.Commit();
                }

                c.Close();
            }
            return true;
        }

        public string GetPasswordHash(Guid userId)
        {
            using (var c = _connectionManager.GetConnection())
            {
                var hash = c.Query<dynamic>("select PasswordHash from ampUser where Id = @id", new {id = userId}).FirstOrDefault();
                c.Close();
                if (hash == null)
                    return null;
                return hash.PasswordHash;
            }
        }

        public bool SetPasswordHash(Guid userId, string hash)
        {

            using (var c = _connectionManager.GetConnection())
            {
                c.Execute("update ampUser set PasswordHash = @hash where Id= @id", new { hash = hash, id = userId });
                c.Close();
            }
            return true;
        }



        public void DeleteRole(string id)
        {
            using (var c = _connectionManager.GetConnection())
            {
                using (var t = c.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {

                        c.Delete<CMSRole>(id, t);

                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                    t.Commit();
                }

                c.Close();
            }

        }

        public CMSRole GetRoleById(string id)
        {
            CMSRole r = null;
            using (var c = _connectionManager.GetConnection())
            {
                r = c.Get<CMSRole>(id);
                c.Close();
            }
            return r;
        }

        public IList<CMSRole> GetRoles()
        {
            List<CMSRole> roles;
            using (var c = _connectionManager.GetConnection())
            {
                roles = c.GetList<CMSRole>().ToList();
                c.Close();
            }
            return roles;
        }

        public IList<CMSRole> GetRoles(Guid userId)
        {
            List<CMSRole> roles;
            using (var c = _connectionManager.GetConnection())
            {
                roles = c.Query<CMSRole>("select Id,Description from ampUserRole  join ampRole on Id = RoleId where UserId = @id",
                    new {id = userId}).ToList();
                c.Close();
            }
            return roles;
        }


        public void CreateRole(CMSRole role)
        {
            using (var c = _connectionManager.GetConnection())
            {
                c.Insert<CMSRole>(role);
                c.Close();
            }
        }

        public void AddUserToRole(Guid userId, string role)
        {
            using (var c = _connectionManager.GetConnection())
            {
                c.Execute("insert into ampUserRole(RoleId,UserId) values(@roleId,@usrId)",
                    new { roleId = role, usrId = userId });
                c.Close();
            }
        }

        public void RemoveUserFromRole(Guid userId, string role)
        {
            using (var c = _connectionManager.GetConnection())
            {
                c.Execute("delete from ampUserRole where RoleId = @roleId and UserId =@userId)",
                    new { roleId = role, userId = userId });
                c.Close();
            }
        }

        public bool IsUserInRole(Guid userId, string roleId)
        {
            using (var c = _connectionManager.GetConnection())
            {
                var record = c.Query<dynamic>("select 1 from ampUserRole where RoleId = @roleId and UserId =@userId",
                    new {roleId = roleId, userId = userId}).FirstOrDefault();
                c.Close();
                return record != null;
            }
        }
    }
}
