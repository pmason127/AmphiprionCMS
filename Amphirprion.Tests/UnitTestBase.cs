using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.Services;
using Moq;
using NUnit.Framework;

namespace Amphirprion.Tests
{
    public delegate Page PageCreator();

    [TestFixture]
    public abstract class PageUnitTestBase
    {

        [TestFixtureSetUp]
        public virtual void Setup()
        {

        }

        protected Page BasePage
        {
            get
            {
                var page = new Page()
                {
                    Id = Guid.NewGuid(),
                    Title = "This is a page Title",
                    HtmlDescription = "<p>This is teh html body</p>",
                    Author = "pmason",
                    CreateDateUtc = DateTime.UtcNow,
                    LastUpdateDateUtc = DateTime.UtcNow,
                    PublishDateUtc = DateTime.UtcNow
                };
                return page;
            }
        }

        protected IPageService GetService(Page p = null)
        {
            var fmt = new Mock<IFormatting>();
            var repo = new Mock<IPageRepository>();

            fmt.Setup(f => f.CreateSlug(It.IsAny<string>(), 50)).Returns((string s, int max) => s);


            repo.Setup(r => r.Get(It.IsAny<Guid>())).Returns((Guid id) =>
            {

                if (p == null)
                {
                    p = BasePage;
                }
              
                return p;


            });

            return new PageService(repo.Object, fmt.Object);
        }
    }
}
