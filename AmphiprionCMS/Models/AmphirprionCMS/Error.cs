using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmphiprionCMS.Models
{
    public class Error
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }
    }
}