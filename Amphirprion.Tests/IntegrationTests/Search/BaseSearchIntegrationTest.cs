using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using NUnit.Framework;
using Directory = System.IO.Directory;
using Version = Lucene.Net.Util.Version;

namespace Amphirprion.Tests.IntegrationTests.Search
{
    [TestFixture]
    public class BaseSearchIntegrationTest
    {
        [TestFixtureSetUp]
        public virtual void Setup()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var assemblyPath = Path.GetDirectoryName(path);
            string outputFolder = Path.Combine(assemblyPath, "Search");

             var directory = FSDirectory.Open(new DirectoryInfo(outputFolder));
            Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
            {
                Index(writer);
                writer.Optimize();
            }

        }

        public virtual void Index(IndexWriter indexer)
        {
            
        }
        [TestFixtureTearDown]
        public void Cleanup()
        {
             string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var assemblyPath = Path.GetDirectoryName(path);
            string outputFolder = Path.Combine(assemblyPath, "Search");

             var directory = FSDirectory.Open(new DirectoryInfo(outputFolder));
            Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);

            using (var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
            {
                writer.DeleteAll();
                writer.Commit();
            }

            Directory.Delete(outputFolder,true);
        }
    }
}
