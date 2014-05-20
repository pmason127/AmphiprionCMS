using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmphiprionCMS.Components;
using NUnit.Framework;

namespace Amphirprion.Tests
{
    public class FormattingTextFixture
    {
        private IFormatting _formatter;
        [TestFixtureSetUp]
        public void Setup()
        {
            _formatter = new Formatting();
        }

        [Test]
        public void SingleSpacesToDashInSlug()
        {
            string input = "i have a new puppy ";
            var slug = _formatter.CreateSlug(input,50);
            Assert.That(slug.Equals("i-have-a-new-puppy",StringComparison.CurrentCulture));
        }
        [Test]
        public void DoubleSpacesToDashInSlug()
        {
            string input = "i  have  a new    puppy ";
            var slug = _formatter.CreateSlug(input,50);
            Assert.That(slug.Equals("i-have-a-new-puppy", StringComparison.CurrentCulture));
        }
        [Test]
        public void TruncateSlugToLength()
        {
            string input = "i have a new puppy ";
            var slug = _formatter.CreateSlug(input,5);
            Assert.That(slug.Equals("i-hav", StringComparison.CurrentCulture));
        }
        [Test]
        public void NonAlphaNumericRemoved()
        {
            string input = "]~i \\%^h[&a@v|e` a? $n)e!w =p_u(}pp+{+y *";
            var slug = _formatter.CreateSlug(input,50);
            Assert.That(slug.Equals("i-have-a-new-puppy", StringComparison.CurrentCulture));
        }
        [Test]
        public void RemoveScript()
        {
            string input = @"Script:<script type=""text/javascript"">alert('hi');</script>Or this:<script type=""text/javascript"">function sayHi(){alert('hi');}</script>";
            var fmt = _formatter.RemoveScript(input);
            Assert.That(fmt.Equals("Script:Or this:", StringComparison.CurrentCulture));
        }
        [Test]
        public void RemoveScriptMultiLine()
        {
            string input = @"Script:<script type=""text/javascript"">
                                        alert('hi');
                                    </script>Or this:
                                    <script type=""text/javascript"">
                                           function sayHi(){
                                               alert('hi');
                                             }
                                    </script>";
            var fmt = _formatter.RemoveScript(input);
            Assert.That(fmt.Equals("Script:Or this:\r\n", StringComparison.CurrentCulture));
        }
    }
}
