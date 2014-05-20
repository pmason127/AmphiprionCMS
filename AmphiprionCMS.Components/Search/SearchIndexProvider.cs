using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data.Entities;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace AmphiprionCMS.Components.Search
{
    public class SearchIndexProvider<T>:ISearchIndexProvider<T> where T:class
    {
        private IDocumentMapper<T> _mapper; 
        public SearchIndexProvider(IDocumentMapper<T> mapper )
        {
            _mapper = mapper;
        }
        public void AddToIndex(T item)
        {
            var document = _mapper.Map(item);
            var indexDoc = GetIndexDocument(document);
        }

        public void RemoveFromIndex(T item)
        {
            throw new NotImplementedException();
        }

        private IndexWriter GetWriter()
        {
            return null;
        }
        private Lucene.Net.Documents.Document GetIndexDocument(Document d)
        {
            if (!d.Fields.Any())
                return null;

            var indexDoc = new Lucene.Net.Documents.Document();
            foreach (var f in d.Fields)
            {
                indexDoc.Add(new Field(f.FieldName,f.Value,f.IsStored ? Field.Store.YES:Field.Store.NO,f.Analyze ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED));
            }

            return indexDoc;
        }
    }

    public class PageMapper:IDocumentMapper<Page>
    {

        public Document Map(Page item)
        {
            throw new NotImplementedException();
        }

        public Page Rehydrate(Document doc)
        {
            throw new NotImplementedException();
        }
    }
}
