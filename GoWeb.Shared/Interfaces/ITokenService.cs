using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Interfaces;

public interface ITokenService
{
    public Task SetTokenAsync(string token);
    public Task<string?> GetTokenAsync();
    public Task RemoveTokenAsync();
}
