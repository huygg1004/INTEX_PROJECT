using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class LoginModel
    {
        public String Id { get; set; }
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        [DataType(DataType.Password)]
        public String Password { get; set; }

    }
}
