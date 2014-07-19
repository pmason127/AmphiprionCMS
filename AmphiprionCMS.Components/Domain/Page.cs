using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Collections.Generic;



namespace Amphiprion.Data.Entities {

    public interface IMenuEnabledContent
    {
        Guid Id { get; }
        string Title { get; set; }
    }

    public static class PageConstants
    {
        private static readonly Guid _defaultPageId = new Guid("9026ee12-6e29-46cc-ba87-72defd40754f");
        public static Guid DefaultPageId { get { return _defaultPageId; } }
    }
    public class    Page:IMenuEnabledContent
    {
   
        public virtual System.Guid? Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string HtmlDescription { get; set; }
        public virtual DateTime? CreateDateUtc { get; set; }
        public virtual DateTime? LastUpdateDateUtc { get; set; }
        public virtual DateTime? PublishDateUtc { get; set; }
        public virtual string Author { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsHomePage { get; set; }
        public virtual string Slug { get; set; }
        public IList<AccessDefinition> AccessDefinition { get; set; }
        public virtual string MetaKeywords { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual System.Nullable<System.Guid> ParentId { get; set; }
        public virtual string Path { get; set; }

        public bool IsPagePublished
        {
            get
            {
                if (!IsApproved)
                    return false;
                if (!PublishDateUtc.HasValue)
                    return true;
                if (PublishDateUtc.Value < DateTime.UtcNow)
                    return true;

                return false;
            }
        }


        Guid IMenuEnabledContent.Id
        {
            get { return Id.Value; }
        }

      
    }

  
    public class AccessDefinition
    {
        public string RoleId { get; set; }
        public Guid PageId { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
		public bool CanPublish { get; set; }
		public bool CanDelete { get; set; }
    }

    public class AccessDefinitionComparer:IEqualityComparer<AccessDefinition>
    {

        public bool Equals(AccessDefinition x, AccessDefinition y)
        {
            return x.RoleId.Equals(y.RoleId, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(AccessDefinition obj)
        {
            return obj.RoleId.GetHashCode();
        }
    }
  
}
