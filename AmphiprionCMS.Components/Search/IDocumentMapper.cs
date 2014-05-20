using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmphiprionCMS.Components.Search
{
    public interface ISearchIndexProvider<T> where T : class 
    {
        void AddToIndex(T item);
        void RemoveFromIndex(T item);
    }

    public interface IDocumentMapper<T> where T:class
    {
        Document Map(T item);
        T Rehydrate(Document doc);
    }

  

    public class Document
    {
        public Document()
        {
            Fields = new List<DocumentField>();
        }
        public IList<DocumentField> Fields { get; private set; } 
    }

    public class DocumentField
    {
        public string FieldName { get; set; }
        public bool IsStored { get; set; }
        public bool Analyze { get; set; }
        public string Value { get; set; }
    }
}
