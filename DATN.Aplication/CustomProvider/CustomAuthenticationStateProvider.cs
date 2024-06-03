using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.CustomProvider
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        IHttpContextAccessor _httpContextAccessor;
        public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string key = _httpContextAccessor.HttpContext.Session.GetString("Key");

            if (key != null)
            {
                var jwtHandle = new JwtSecurityTokenHandler();
                var jwtSecurityToken = jwtHandle.ReadJwtToken(key);
                var claims = jwtSecurityToken.Claims;
                var identity = new ClaimsIdentity(claims, "JwtBearer");
                _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(identity);
                return new AuthenticationState(_httpContextAccessor.HttpContext.User);
            }
            else
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}
