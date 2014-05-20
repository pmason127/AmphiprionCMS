using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace Amphirprion.Tests.IntegrationTests.Search
{
    public class IndexingTestFixture : BaseSearchIntegrationTest
    {
        public override void Index(IndexWriter indexer)
        {
           Document d= new Document();
            d.Add(new Field("id","1234",Field.Store.YES,Field.Index.NOT_ANALYZED));
            indexer.AddDocument(d);
        }
    }
}
