using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmphiprionCMS.Components.Security;
using Amphirprion.Tests.IntegrationTests.localDb;
using NUnit.Framework;

namespace Amphirprion.Tests.IntegrationTests
{
    public class UserCreationTestFixture:SQLBaseIntegrationTest
    {
        private CMSUser user1;
        CMSUser user2;

        public override void BeforeTests()
        {
            var id = Guid.NewGuid();
             user1 = new CMSUser(){Id=id,Email = id.ToString() + "@localhost.com",UserName = "u" + id.ToString(),IsActive =true};
            UserRepository.CreateUser(user1);

            user2 = UserRepository.GetUser(user1.Id);
        }

        [Test]
        public void UserIsCreated()
        {
            Assert.NotNull(user2);
        }
        [Test]
        public void EmailIsSet()
        {
            Assert.IsTrue(user1.Email.Equals(user2.Email));
        }
        [Test]
        public void IdIsSet()
        {
            Assert.IsTrue(user1.Id.Equals(user2.Id));
        }
        [Test]
        public void UserNameIsSet()
        {
            Assert.IsTrue(user1.UserName.Equals(user2.UserName));
        }
        [Test]
        public void IsActiveIsSet()
        {
            Assert.IsTrue(user2.IsActive);
        }

        [Test]
        public void CanGetUserByEmail()
        {
            var u = UserRepository.GetUserByEmail(user2.Email);
            Assert.NotNull(u);
        }
        [Test]
        public void CanGetUserByUsername()
        {
            var u = UserRepository.GetUser(user2.UserName);
            Assert.NotNull(u);
        }
        [Test]
        public void CanSetPasswordHash()
        {
            UserRepository.SetPasswordHash(user2.Id, "password");
            var hash = UserRepository.GetPasswordHash(user2.Id);
            Console.Write(hash);
            Assert.IsNotNullOrEmpty(hash);
        }
        
    }

    public class UserInactiveGetFixture : SQLBaseIntegrationTest
    {
        private CMSUser user1;
        CMSUser user2;

        public override void BeforeTests()
        {
            var id = Guid.NewGuid();
            user1 = new CMSUser() { Id = id, Email = id.ToString() + "@localhost.com", UserName = "u" + id.ToString(), IsActive = false };
            UserRepository.CreateUser(user1);

            user2 = UserRepository.GetUser(user1.Id);
        }
        [Test]
        public void UserIsNull()
        {
            Assert.IsNull(user2);
        }
        [Test]
        public void CannotGetUserByEmail()
        {
            var u = UserRepository.GetUserByEmail(user1.Email);
            Assert.IsNull(u);
        }
        [Test]
        public void CannotGetUserByUsername()
        {
            var u = UserRepository.GetUser(user1.UserName);
            Assert.IsNull(u);
        }
    }

    public class RoleTestFixture:SQLBaseIntegrationTest
    {
        private CMSRole role;
        private CMSRole role2;
        public override void BeforeTests()
        {
            Guid testId = Guid.NewGuid();
            role = new CMSRole(){Description ="A test role",Id=testId.ToString()};
            UserRepository.CreateRole(role);

            role2 = UserRepository.GetRoleById(role.Id);
        }

        [Test]
        public void RoleIsCreated()
        {
            Assert.IsNotNull(role2);
        }
        [Test]
        public void RoleIdAndNameMatch()
        {
            Assert.IsTrue(role2.Id.Equals(role2.Name));
        }
        [Test]
        public void NameIsSet()
        {
            Assert.IsTrue(role2.Name.Equals(role.Name));
        }
        [Test]
        public void IdIsSet()
        {
            Assert.IsTrue(role2.Id.Equals(role.Id));
        }
        [Test]
        public void DescriptionIsSet()
        {
            Assert.IsTrue(role2.Description.Equals(role.Description));
        }
    }

    public class UserRoleTestFixture : SQLBaseIntegrationTest
    {
        private CMSUser user;
        private CMSRole role;
        private CMSRole role2;
        public override void BeforeTests()
        {
            var id = Guid.NewGuid();
            user = new CMSUser() { Id = id, Email = id.ToString() + "@localhost.com", UserName = "u" + id.ToString(), IsActive = true };
            UserRepository.CreateUser(user);
            var roleId = Guid.NewGuid().ToString();
            role = new CMSRole(){Id=roleId};
            UserRepository.CreateRole(role);
            var roleId2 = Guid.NewGuid().ToString();
            role2 = new CMSRole() { Id = roleId2 };
            UserRepository.CreateRole(role2);

            UserRepository.AddUserToRole(user.Id, role.Id);
            UserRepository.AddUserToRole(user.Id, role2.Id);
        }

        [Test]
        public void UserAddedToRoles()
        {
           
            var lists = UserRepository.GetRoles(user.Id);
            Assert.AreEqual(lists.Count,2);
        }
        [Test]
        public void CannotReaddSameRoles()
        {
            Exception ex = null;
            try
            {
                UserRepository.AddUserToRole(user.Id, role.Id);
            }
            catch (Exception  e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
        }
     }

}
