using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Intex_app.Infrastructure;

namespace Intex_app.Models
{
    public class Token
    {
        public string TokenString { get; set; }
    }
    public class SessionToken : Token
    {
        public static SessionToken GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionToken token = session?.GetJason<SessionToken>("Token")
                                 ?? new SessionToken();
            token.Session = session;
            return token;
        }
        [JsonIgnore]
        public ISession Session { get; set; }
    }
}
