using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Components.Security;
using Dapper;
using DapperExtensions;


namespace Amphiprion.Data
{
    public interface IPageRepository
    {
        void Create(Page page);
        void Update(Page page);
        Page Get(Guid id);
        Page Get(string url);
        Page GetHomePage();
        void Delete(Guid id);
        IList<Page> List(bool publishedonly =false,bool activeOnly=false);
        IList<AccessDefinition> GetPageAccessDefinition(Guid pageId);
        void SetAccessDefinition(Guid pageId, IList<AccessDefinition> definitions);
        Page GetBySlug(string slug);
    }

    public class PageRepository : IPageRepository
    {
        private IConnectionManager _connectionManager = null;
        public PageRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public void Create(Page page)
        {
            using (var con = _connectionManager.GetConnection())
            {
                using (var t = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        con.Insert<Page>(page, t);
                        if (page.IsHomePage)
                        {
                            con.Execute("update ampPage set IsHomePage = 0 where IsHomePage = 1 and Id <> @id",
                                new { id = page.Id }, t);
                        }

                    }
                    catch (Exception)
                    {
                      t.Rollback();
                        throw;
                    }
                    t.Commit();
                    con.Close();
                }
              
            }
        }

        public void Update(Page page)
        {
            using (var con = _connectionManager.GetConnection())
            {
                using (var t = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        con.Update<Page>(page,t);
                        if (page.IsHomePage)
                        {
                            con.Execute("update ampPage set IsHomePage = 0 where IsHomePage = 1 and Id <> @id",
                                new {id = page.Id}, t);
                        }
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                    t.Commit();
                    con.Close();
                }
            }
        }

        public Page Get(Guid id)
        {
            Page node = null;
            using (var con = _connectionManager.GetConnection())
            {
                 node = con.Get<Page>(id);
                con.Close();
            }
            return node;
        }
        public Page GetHomePage()
        {
            Page node = null;
            using (var con = _connectionManager.GetConnection())
            {
                var pr = Predicates.Field<Page>(p => p.IsHomePage, Operator.Eq, true);
                var pages = con.GetList<Page>(pr);
                node = pages.FirstOrDefault();
                con.Close();
            }
            return node;
        }
        public Page Get(string url)
        {
            Page node = null;
            var pr = Predicates.Field<Page>(p => p.Url, Operator.Eq, url.ToLower());
            using (var con = _connectionManager.GetConnection())
            {
                var nodes = con.GetList<Page>(pr);
                node = nodes.FirstOrDefault();
                con.Close();
            }
            return node;
        }
        public Page GetBySlug(string slug)
        {
            Page node = null;
            var pr = Predicates.Field<Page>(p => p.Slug, Operator.Eq, slug.ToLower());
            using (var con = _connectionManager.GetConnection())
            {
                var nodes = con.GetList<Page>(pr);
                node = nodes.FirstOrDefault();
                con.Close();
            }
            return node;
        }
        public void Delete(Guid id)
        {
            using (var con = _connectionManager.GetConnection())
            {
                var pred = Predicates.Field<Page>(p => p.Id, Operator.Eq, id);
                con.Delete<Page>(pred);
                con.Close();
            }
        }

        public IList<Page> List(bool publishedonly =false,bool activeOnly=false)
        {
            //using (var con = _connectionManager.GetConnection())
            //{
            //    var nodes = con.GetList<T>(()
            //    node = nodes.FirstOrDefault();
            //    con.Close();
            //}
            return null;
        }


        public IList<AccessDefinition> GetPageAccessDefinition(Guid pageId)
        {
            IList<AccessDefinition> definitions;
            var pr = Predicates.Field<AccessDefinition>(p => p.PageId, Operator.Eq, pageId);
            using (var con = _connectionManager.GetConnection())
            {
                definitions = con.GetList<AccessDefinition>(pr).ToList();
             
                con.Close();
            }
            return definitions;
        }

        public void SetAccessDefinition(Guid pageId, IList<AccessDefinition> definitions = null)
        {
             var pr = Predicates.Field<AccessDefinition>(p => p.PageId, Operator.Eq, pageId);
            using (var con = _connectionManager.GetConnection())
            {
                using (var t = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        con.Delete<AccessDefinition>(pr, t);
                        if (definitions != null)
                        {
                            con.Insert<AccessDefinition>(definitions, t);
                        }
                        t.Commit();
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                  
                }
              
                con.Close();
            }
        }
    }
}
