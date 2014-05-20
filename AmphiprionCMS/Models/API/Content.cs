using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmphiprionCMS.Models.Api
{
    public class Content
    {
        AmphiprionCMS.Data.Entities.Content _content;
        public Content(AmphiprionCMS.Data.Entities.Content content)
        {
            _content = content;
        }
        public Guid Id { get { return _content.Id; } }
        public Guid TypeId { get { return _content.TypeId; } }
        public string Title { get { return _content.Title; } }
        public string HtmlDescription { get { return _content.HtmlDescription; } }
        public DateTime CreateDateUtc { get { return _content.CreateDateUtc; } }
        public DateTime LastUpdateDateUtc { get { return _content.LastUpdateDateUtc; } }
        public string Author { get { return _content.Author; } }
        public int? Version { get { return _content.Version; } }
        public bool IsActive { get { return _content.IsActive; } }
        public Guid? ParentId { get { return _content.ParentId; } }
        public bool IsApproved { get { return _content.IsApproved; } }

        public Content Parent
        {
            get
            {
                if (_content.ParentContent == null)
                    return null;
                return  new Content(_content.ParentContent);
            }
        }

        public SimpleContentType ContentType
        {
            get
            {
                return new SimpleContentType(_content.ContentType);
            }
        }
    }

    public class ContentType
    {
        private AmphiprionCMS.Data.Entities.ContentType _type;
        public ContentType(AmphiprionCMS.Data.Entities.ContentType type)
        {
            _type = type;
        }
        public Guid Id { get { return _type.Id; } }
        public bool IsEnabled { get { return _type.IsEnabled; } }
        public string Name { get { return _type.Name; } }
        public bool SupportsVersioning { get { return _type.SupportsVersioning; } }

        public IList<SimpleContentType> AllowedContentTypes
        {
            get
            {
                return null;
            }
        }
    }

    public class SimpleContentType
    {
        private AmphiprionCMS.Data.Entities.ContentType _type;
        public SimpleContentType(AmphiprionCMS.Data.Entities.ContentType type)
        {
            _type = type;
        }
        public Guid Id { get { return _type.Id; } }
        public string Name { get { return _type.Name; } }
    }
}