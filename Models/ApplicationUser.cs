using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string ClientID { get; set; }
        public bool Admin { get; set; }
        public bool Editor { get; set; }

        public async Task<IdentityResult> GenerateUserIdentiyAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userResult = await manager.CreateAsync(this, authenticationType);

            return userResult;
        }

        public string GetUserId()
        {
            return Id;
        }
        public void makeEditor()
        {
            Editor = true;
            return;
        }
    }
}
