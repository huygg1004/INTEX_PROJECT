using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class StorageUploadForm
    {
        [Required]
        public IFormFile photofile { get; set; }
    }
}
