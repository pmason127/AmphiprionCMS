using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data.Entities;
using NUnit.Framework;
namespace Amphirprion.Tests.IntegrationTests.localDb
{
    #region GenericCreateRetrievalTest
    public abstract  class PageCreationFixture : SQLBaseIntegrationTest
    {
        private Page retrievedPage;
        private Page createdPage;
        private Guid id;
        public Page RetrievedPage { get { return retrievedPage; } }
        public Page CreatedPage { get { return createdPage; } }
        public override void BeforeTests()
        {
            createdPage = CreatePageAction();
            if (createdPage == null) return;
            retrievedPage = Repository.Get(createdPage.Id.Value);
        }

        public virtual Page CreatePageAction()
        {
            return null;
        }
        [Test]
        public void PageExists()
        {
            Assert.IsNotNull(retrievedPage);
        }
        [Test]
        public void IdValid()
        {
            Assert.AreEqual(createdPage.Id, retrievedPage.Id);
        }
        [Test]
        public void TitleValid()
        {
            Assert.AreEqual(createdPage.Title, retrievedPage.Title);
        }
        [Test]
        public void DescriptionValid()
        {
            Assert.AreEqual(createdPage.HtmlDescription, retrievedPage.HtmlDescription);
        }
        [Test]
        public void CreateValid()
        {
            Assert.AreEqual(createdPage.CreateDateUtc.ToString(), retrievedPage.CreateDateUtc.ToString());
        }
        [Test]
        public void UpdateValid()
        {
            Assert.AreEqual(createdPage.LastUpdateDateUtc.ToString(), retrievedPage.LastUpdateDateUtc.ToString());
        }
        [Test]
        public void PublishedValid()
        {
            Assert.AreEqual(createdPage.PublishDateUtc.ToString(), retrievedPage.PublishDateUtc.ToString());
        }
        [Test]
        public void AuthorValid()
        {
            Assert.AreEqual(createdPage.Author, retrievedPage.Author);
        }
        [Test]
        public void ApprovedValid()
        {
            Assert.AreEqual(createdPage.IsApproved, retrievedPage.IsApproved);
        }
        [Test]
        public void ActiveValid()
        {
            Assert.AreEqual(createdPage.IsActive, retrievedPage.IsActive);
        }
        [Test]
        public void SlugValid()
        {
            Assert.AreEqual(createdPage.Slug, retrievedPage.Slug);
        }
       
       
        [Test]
        public void KeywordValid()
        {
            Assert.AreEqual(createdPage.MetaKeywords, retrievedPage.MetaKeywords);
        }
        [Test]
        public void MetaDescriptionValid()
        {
            Assert.AreEqual(createdPage.MetaDescription, retrievedPage.MetaDescription);
        }
        [Test]
        public void ParentValid()
        {
            Assert.AreEqual(createdPage.ParentId, retrievedPage.ParentId);
        }
       
    }
    #endregion

    public class CanCreatePageAllProperties:PageCreationFixture
    {
        public override Page CreatePageAction()
        {
             Guid id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var temp = new Page()
            {
                Id = id,
                Title = "This is a page Title",
                HtmlDescription = "<p>This is the html body</p>",
                Author = "pmason",
                CreateDateUtc = now,
                IsActive = true,
                IsApproved = true,
                PublishDateUtc = now,
                LastUpdateDateUtc = now,              
                Slug = "a-test-page",
                ParentId =PageConstants.DefaultPageId,
                MetaDescription = "This is a description",
                MetaKeywords = "one,two,three",

            };

            Repository.Create(temp);

            return temp;
        }

        [Test]
        public void PathIsSet()
        {
            Assert.IsNotNullOrEmpty(RetrievedPage.Path);
        }
        [Test]
        public void PathIsCorrect()
        {
            Assert.AreEqual(RetrievedPage.Path,"/a-test-page");
        }
    }
    public class CanCreatePageLevel2 : PageCreationFixture
    {
        public override Page CreatePageAction()
        {
            Guid id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var temp = new Page()
            {
                Id = id,
                Title = "This is a page Title",
                HtmlDescription = "<p>This is the html body</p>",
                Author = "pmason",
                CreateDateUtc = now,
                IsActive = true,
                IsApproved = true,
                PublishDateUtc = now,
                LastUpdateDateUtc = now,
                Slug = "a-test-page",
                ParentId = PageConstants.DefaultPageId,
                MetaDescription = "This is a description",
                MetaKeywords = "one,two,three",

            };

            Repository.Create(temp);

            Guid id2 = Guid.NewGuid();
        
            var temp2 = new Page()
            {
                Id = id2,
                Title = "This is a page Title Level 2",
                HtmlDescription = "<p>This is the html body</p>",
                Author = "pmason",
                CreateDateUtc = now,
                IsActive = true,
                IsApproved = true,
                PublishDateUtc = now,
                LastUpdateDateUtc = now,
                Slug = "a-test-page-2",
                ParentId = id,
                MetaDescription = "This is a description",
                MetaKeywords = "one,two,three",

            };

            Repository.Create(temp2);

            return temp2;
        }

        [Test]
        public void PathIsSet()
        {
            Assert.IsNotNullOrEmpty(RetrievedPage.Path);
        }
        [Test]
        public void PathIsCorrect()
        {
            Assert.AreEqual(RetrievedPage.Path, "/a-test-page/a-test-page-2");
        }
    }
    public class CanCreatePageLevel3 : PageCreationFixture
    {
        public override Page CreatePageAction()
        {
            Guid id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var temp = new Page()
            {
                Id = id,
                Title = "This is a page Title",
                HtmlDescription = "<p>This is the html body</p>",
                Author = "pmason",
                CreateDateUtc = now,
                IsActive = true,
                IsApproved = true,
                PublishDateUtc = now,
                LastUpdateDateUtc = now,
                Slug = "a-test-page",
                ParentId = PageConstants.DefaultPageId,
                MetaDescription = "This is a description",
                MetaKeywords = "one,two,three",

            };

            Repository.Create(temp);

            Guid id2 = Guid.NewGuid();

            var temp2 = new Page()
            {
                Id = id2,
                Title = "This is a page Title Level 2",
                HtmlDescription = "<p>This is the html body</p>",
                Author = "pmason",
                CreateDateUtc = now,
                IsActive = true,
                IsApproved = true,
                PublishDateUtc = now,
                LastUpdateDateUtc = now,
                Slug = "a-test-page-2",
                ParentId = id,
                MetaDescription = "This is a description",
                MetaKeywords = "one,two,three",

            };

            Repository.Create(temp2);

            Guid id3 = Guid.NewGuid();

            var temp3 = new Page()
            {
                Id = id3,
                Title = "This is a page Title Level 2",
                HtmlDescription = "<p>This is the html body</p>",
                Author = "pmason",
                CreateDateUtc = now,
                IsActive = true,
                IsApproved = true,
                PublishDateUtc = now,
                LastUpdateDateUtc = now,
                Slug = "a-test-page-3",
                ParentId = id2,
                MetaDescription = "This is a description",
                MetaKeywords = "one,two,three",

            };

            Repository.Create(temp3);

            return temp3;
        }

        [Test]
        public void PathIsSet()
        {
            Assert.IsNotNullOrEmpty(RetrievedPage.Path);
        }
        [Test]
        public void PathIsCorrect()
        {
            Assert.AreEqual(RetrievedPage.Path, "/a-test-page/a-test-page-2/a-test-page-3");
        }
    }
    public class CanCreatePageMinimumProperties : PageCreationFixture
    {
        private DateTime now;
        public override Page CreatePageAction()
        {
            Guid id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var temp = new Page()
            {
                Id = id,
                Title = "This is a page Title",
                HtmlDescription = "<p>This is teh html body</p>",
                Author = "pmason",
                Slug = "a-test-page",
                Path = "home/page",
                CreateDateUtc = now,
                LastUpdateDateUtc  = now,
                PublishDateUtc = now
            };
            Repository.Create(temp);

            return temp;
        }

        public void IsActiveIsTrue()
        {
            Assert.IsFalse(RetrievedPage.IsActive);
        }
        [Test]
        public void IsApprovedTrue()
        {
            Assert.IsFalse(RetrievedPage.IsApproved);
        }
    }

  
    public class PageUpdateOptions : PageCreationFixture
    {
        public override Page CreatePageAction()
        {
            Guid id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var temp = new Page()
            {
                Id = id,
                Title = "This is a page Title",
                HtmlDescription = "<p>This is teh html body</p>",
                Author = "pmason",
                CreateDateUtc = now,
                IsActive = true,
                IsApproved = true,
                PublishDateUtc = now,
                LastUpdateDateUtc = now,
                Slug = "a-test-page",
                Path = "home/page",
                ParentId = null,
                MetaDescription = "This is a description",
                MetaKeywords = "one,two,three",

            };

            Repository.Create(temp);


            temp.Title = "This is a page Title Changed";
            temp.HtmlDescription = "<p>Body Change</p>";
            temp.Author = "pmason2";
            temp.CreateDateUtc = now.AddHours(6);
            temp.IsActive = false;
            temp.IsApproved = false;
            temp.PublishDateUtc = now.AddHours(6); 
            temp.LastUpdateDateUtc = now.AddHours(6); 
            temp.Slug = "updated-page";
            temp.Path = "updated/page";
            temp.ParentId = null;
            temp.MetaDescription = "This is a description altered";
            temp.MetaKeywords = "one,two,three,four";
            Repository.Update(temp);
            return temp;
        }
    }

    
    public class PageDelete:SQLBaseIntegrationTest
    {
        private Page p;

        public override void BeforeTests()
    {
            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var temp = new Page()
            {
                Id = id,
                Title = "This is a page Title",
                HtmlDescription = "<p>This is teh html body</p>",
                Author = "pmason",
                CreateDateUtc = now,
                IsActive = true,
                IsApproved = true,
                PublishDateUtc = now,
                LastUpdateDateUtc = now,
                Slug = "a-test-page",
                Path = "home/page",
                ParentId = null,
                MetaDescription = "This is a description",
                MetaKeywords = "one,two,three",

            };

            Repository.Create(temp);

             p = Repository.Get(id);
        }

        [Test]
        public void CanDeletePage()
        {
            Repository.Delete(p.Id.Value);
            var p2 = Repository.Get(p.Id.Value);
            Assert.IsNull(p2);
        }
    }
}
