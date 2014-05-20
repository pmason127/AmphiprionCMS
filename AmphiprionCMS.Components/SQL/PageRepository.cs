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
using Lucene.Net.Util;


namespace Amphiprion.Data
{
    public interface IPageRepository
    {
        void Create(Page page);
        void Update(Page page);
        Page Get(Guid id);
        Page Get(string path);
        Page GetHomePage();
        void Delete(Guid id);
        IList<Page> List(Guid? parentId = null, bool includeParentInResults = false, bool incudeUnpublished = false, bool includeInactive = false);
        IList<AccessDefinition> GetPageAccessDefinition(Guid pageId);
        void SetAccessDefinition(Guid pageId, IList<AccessDefinition> definitions);
        Page GetBySlug(string slug);
    }

    public class PageRepository : IPageRepository
    {
        private readonly string recursiveSQL = @"
;with basePath(Id,TreePath)
AS
(
  Select Id, cast(null as nvarchar(512)) as TreePath from ampPage where ParentId is null
  union all
  select e.Id
  ,CAST(isnull(f.TreePath,'') + cast('/' as nvarchar(1)) + CAST(e.Slug AS nvarchar(512)) AS nvarchar(512)) as TreePath 
  from ampPage e inner join basePath f on f.Id = e.ParentId
)
";
        private IConnectionManager _connectionManager = null;
        public PageRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public void Create(Page page)
        {
            var path = GetBasePath(page.ParentId, page.Slug);
            using (var con = _connectionManager.GetConnection())
            {
                using (var t = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        
                        page.Path = path;
                        con.Insert<Page>(page, t);
                     
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
                        //if (page.IsHomePage)
                        //{
                        //    con.Execute("update ampPage set IsHomePage = 0 where IsHomePage = 1 and Id <> @id",
                        //        new {id = page.Id}, t);
                        //}
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
            //using (var con = _connectionManager.GetConnection())
            //{
            //    var pr = Predicates.Field<Page>(p => p.IsHomePage, Operator.Eq, true);
            //    var pages = con.GetList<Page>(pr);
            //    node = pages.FirstOrDefault();
            //    con.Close();
            //}
            return node;
        }
        public Page Get(string path)
        {
            Page node = null;
            var pr = Predicates.Field<Page>(p => p.Path, Operator.Eq, path.ToLower());
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

                if(id == PageConstants.DefaultPageId)
                    throw new ApplicationException("Cannot delete home page.  Add home page reference instead");

                con.Delete<Page>(pred);
                con.Close();
            }
        }

        public IList<Page> List(Guid? parentId = null, bool includeParentInResults = false, bool includeUnpublished = false, bool includeInactive = false)
        {
            IList<Page> pages = new List<Page>();
            using (var con = _connectionManager.GetConnection())
            {
                var sort = new List<ISort>()
                {
                    Predicates.Sort<Page>(p => p.Path, true)
                };

                PredicateGroup mainGroup = new PredicateGroup()
                {
                    Operator = GroupOperator.And,
                    Predicates = new List<IPredicate>()
                };

                PredicateGroup gr = new PredicateGroup(){Operator = GroupOperator.And,Predicates = new List<IPredicate>()};
                mainGroup.Predicates.Add(gr);

                if (!includeUnpublished)
                {
                    gr.Predicates.Add(Predicates.Field<Page>(p => p.PublishDateUtc, Operator.Eq, null, true));
                    gr.Predicates.Add(Predicates.Field<Page>(p=>p.IsApproved,Operator.Eq,true));
                }
                if (!includeInactive)
                {
                    gr.Predicates.Add(Predicates.Field<Page>(p => p.IsActive, Operator.Eq, true));
                }
                if (parentId.HasValue)
                {
                    PredicateGroup gr2 = new PredicateGroup() { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                    gr2.Predicates.Add(Predicates.Field<Page>(p=>p.ParentId,Operator.Eq, parentId.Value));
                    if(includeParentInResults)
                        gr2.Predicates.Add(Predicates.Field<Page>(p => p.Id, Operator.Eq, parentId.Value));

                    mainGroup.Predicates.Add(gr2);
                }

                pages = con.GetList<Page>(mainGroup, sort).ToList();
                con.Close();
            }
            return pages;
        }

        private string GetBasePath(Guid? parentId, string slug)
        {
          
            using (var con = _connectionManager.GetConnection())
            {
                var sql = recursiveSQL + " select TreePath from basePath where Id = @id";

                var paths =  con.Query<string>(sql, new {id = parentId});
               var path = paths.FirstOrDefault();
                return string.Concat(path, "/", slug);
            }
            
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
