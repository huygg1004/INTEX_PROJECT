using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Services
{
    public interface S3interface
    {
        Task<string> AddItem(IFormFile file, string readerName);
    };
}
