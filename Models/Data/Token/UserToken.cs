using System;
using System.Collections;

namespace CorporativeSystem.Controllers.Models.Data
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime ? Expiration { get; set; }
        public bool Authenticated { get; set; }      
    }
}
