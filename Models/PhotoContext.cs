using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class PhotoContext: DbContext
    {
        public PhotoContext (DbContextOptions<PhotoContext> options) : base(options)
        {

        }

        public PhotoContext()
        {

        }

        public static PhotoContext Create()
        {
            return new PhotoContext();
        }

        public DbSet<Photo> Photos { get; set; }
    }
}
