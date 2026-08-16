using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Model
{
    public class TokenDTO
    {
       public string Value { get; set; } = string.Empty;
       public DateTime Expiration { get; set; }
    }
}
