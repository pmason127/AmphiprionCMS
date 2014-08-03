using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation;

namespace AmphiprionCMS.Models
{
    [FluentValidation.Attributes.Validator(typeof(SetupModelValidator))]
    public class SetupModel
    {
        [DisplayName("Raw Sql Connection String")]
        public string ConnectionString
        {
            get; set;
        }
         [DisplayName("Connection String Name")]
        public string ConnectionStringName
        {
            get;
            set;
        }
          [DisplayName("Admininstrative User Name")]
        public string Username
        {
            get;
            set;
        }
          [DisplayName("Admininstrative Email Address")]
        public string EmailAddress
        {
            get;
            set;
        }
        [DataType(DataType.Password)]
        [DisplayName("Admininstrative Password")]
        public string Password
        {
            get;
            set;
        }
        
        [DisplayName("Execute SQL")]
        [Description("Execute the SQL on your database automatically, requires Dbo privileges")]
        public bool ExecuteSql
        {
            get;
            set;
        }
        
    }

    public class SetupModelValidator : AbstractValidator<SetupModel>
    {

        public SetupModelValidator()
        {
            RuleFor(p => p.Username).NotEmpty();
            RuleFor(p => p.Password).NotEmpty();
            RuleFor(p => p.EmailAddress).NotEmpty().EmailAddress();
            RuleFor(p => p.ConnectionString).NotEmpty().When(m => string.IsNullOrEmpty(m.ConnectionStringName)).WithMessage("Either connection string or connection string name must be set");
            RuleFor(p => p.ConnectionStringName).NotEmpty().When(m => string.IsNullOrEmpty(m.ConnectionString)).WithMessage("Either connection string or connection string name must be set");
        }
    }
}
