using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Repositories;
using GoWeb.Сonstants.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GoWeb.Shared.Models;
using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;

namespace GoWeb.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;  
            this.configuration = configuration;
        }

        [HttpPost]
        [Route(LoginRequest.RouteTemplate)]
        public async Task<ActionResult<LoginRequest.Response>> login(UserLoginDTO loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userRepository.FindByNameAsync(loginViewModel.UserName);
                if (user != null)
                {
                    var result = await userRepository.CheckPasswordAsync(user, loginViewModel.Password);
                    if (result)
                    {

                        var authClaims = new List<Claim>
                                    {
                                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                                        new Claim(ClaimTypes.Name, user.UserName! ?? user.Email!),
                                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                                    };
                        var userRoles = await userRepository.GetRolesAsync(user);
                        foreach (var role in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        authClaims.AddRange(ClaimsConst.GetClaims(user));
                        var jwtSettings = configuration.GetSection("JwtSettings"); //Читаем настройки JWT из appsettings.json
                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
                        var token = new JwtSecurityToken(
                                                        issuer: jwtSettings["Issuer"],
                                                        audience: jwtSettings["Audience"],
                                                        expires: DateTime.UtcNow.AddHours(3), // Токен будет жить 3 часа
                                                        claims: authClaims,
                                                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                                                    );

                        return new LoginRequest.Response(new TokenDTO { Value = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo });

                    }
                }
                return Unauthorized();
            }
            return BadRequest();

        }


    }
}
