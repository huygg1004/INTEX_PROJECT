using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class Photo
    {   
        [Key]
        public int PhotoID { get; set; }
        [NotMapped]
        public IFormFile uploadphoto { get; set; }
        public string photoUrl { get; set; }
        public string BurialSiteID { get; set; }
    }
}
