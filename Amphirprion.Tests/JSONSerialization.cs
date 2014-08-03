using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Amphirprion.Tests
{
    [TestFixture]
    public class JSONSerialization
    {
        [Test]
        public void CanDeserializeJSON()
        {
            var str = "{\"Prop\":1,\"Prop2\":\"test\"}";

           
            dynamic obj = JObject.Parse( str);
        }
    }
}
