using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorporativeSystem.Models.Data.Token
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpirationHours { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }
    }
}
