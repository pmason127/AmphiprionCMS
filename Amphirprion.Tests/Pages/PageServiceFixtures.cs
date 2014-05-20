using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.Services;
using Amphirprion.Tests.IntegrationTests.localDb;
using Moq;
using NUnit.Framework;

namespace Amphirprion.Tests.Pages
{
    [TestFixture]
    public class MimimumPageCreation
    {
        private IPageService _service;
        private Page page;
        [TestFixtureSetUp]
        public void Setup()
        {
            var data = new Mock<IPageRepository>();
            var fmt = new Mock<IFormatting>();
            _service = new PageService(data.Object,fmt.Object);
            Guid id = Guid.NewGuid();
            page = new Page()
            {
                Id = id,
                Title = "This is a page Title",
                HtmlDescription = "<p>This is teh html body</p>",
                Author = "pmason",
                Slug = "a-test-page",
                Path = "home/page",
 
            };
            _service.CreatePage(page);
        }

        [Test]
        public void CreateDateIsSet()
        {
            Assert.IsTrue(page.CreateDateUtc.HasValue);
        }
        [Test]
        public void CreateDateIsEqualToLastUpdateSet()
        {
            Assert.AreEqual(page.CreateDateUtc.ToString(), page.LastUpdateDateUtc.ToString());
        }
    }
    [TestFixture]
    public class RequiredFields
    {
        private IPageService _service;
        private Page page;
        [TestFixtureSetUp]
        public void Setup()
        {
            var data = new Mock<IPageRepository>();
            data.Setup(d => d.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => new Page() {Id = id, Title = "Test", HtmlDescription = "Body"});
            var fmt = new Mock<IFormatting>();
            _service = new PageService(data.Object,fmt.Object);
           
        }

        [Test]
        public void TitleIsRequired()
        {
            Guid id = Guid.NewGuid();
            bool argException = false;
            page = new Page()
            {
                Id = id,
                HtmlDescription = "<p>This is teh html body</p>",
                Author = "pmason",
                Slug = "a-test-page",
                Path = "home/page",

            };
            try
            {
                _service.CreatePage(page);
            }
            catch (ArgumentException ex)
            {
                argException = ex.ParamName == "Title";
            }
            catch (Exception ex)
            {
                
            }
            Assert.IsTrue(argException);
            
        }
        [Test]
        public void HtmlDescriptionIsRequired()
        {
            Guid id = Guid.NewGuid();
            bool argException = false;
            page = new Page()
            {
                Id = id,
                Title = "Title",
                Author = "pmason",
                Slug = "a-test-page",
                Path = "home/page",

            };
            try
            {
                _service.CreatePage(page);
            }
            catch (ArgumentException ex)
            {
                argException = ex.ParamName == "HtmlDescription";
            }
            catch (Exception ex)
            {

            }
            Assert.IsTrue(argException);

        }
    }

    [TestFixture]
    public class PageSlugAndUrlOptions:PageUnitTestBase
    {
      
        [Test]
        public void SlugIsGenerated()
        {
            var p = new Page() {Title = "testslugcreate", HtmlDescription = "Body"};
            GetService().CreatePage(p);
            Assert.IsNotNullOrEmpty(p.Slug);
        }
        [Test]
        public void UrlNoParentIsGenerated()
        {
            var p = new Page() { Title = "testslugcreate", HtmlDescription = "Body" };
            GetService().CreatePage(p);
            Assert.IsTrue(p.Path.Equals(p.Slug));
        }
        [Test]
        public void UrlWithParentIsGenerated()
        {
            var parent = BasePage;
            parent.Path = "parentpage";
            var p = new Page() { Title = "testslugcreate", HtmlDescription = "Body",ParentId = Guid.NewGuid() };
            GetService(parent).CreatePage(p);
            Assert.IsTrue(p.Path.Equals("parentpage/testslugcreate"));
        }
    }
    [TestFixture]
    public class PageServiceDeleteOptions
    {
      //  private IPageService _service;
        [Test]
        public void CannotDeleteHomepage()
        {
            Exception exc = null;
            var repo = new Mock<IPageRepository>();
            var fmt = new Mock<IFormatting>();
            repo.Setup(r => r.Get(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                return new Page(){Id= id};
            });

            IPageService service = new PageService(repo.Object,fmt.Object);
            try
            {
                service.DeletePage(Guid.NewGuid());
            }
            catch (Exception ex)
            {
                exc = ex;
            }
            Assert.IsInstanceOf<InvalidOperationException>(exc);
        }
        [Test]
        public void CanDeletePage()
        {
            Exception exc = null;
            var repo = new Mock<IPageRepository>();
            var fmt = new Mock<IFormatting>();
            repo.Setup(r => r.Get(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                return new Page() { Id = id };
            });

            IPageService service = new PageService(repo.Object,fmt.Object);
            try
            {
                service.DeletePage(Guid.NewGuid());
            }
            catch (Exception ex)
            {
                exc = ex;
            }
            Assert.IsNull(exc);
        }
    }

  
    
}
