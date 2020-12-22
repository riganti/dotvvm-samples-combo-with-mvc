using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotvvmMvcIntegration
{
    public class CustomJwtOptions
    {

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }

    }
}
