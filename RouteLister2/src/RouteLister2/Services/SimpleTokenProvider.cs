using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Identity;
    using Models;

    namespace SimpleTokenProvider
    {
        public class TokenProviderMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly TokenProviderOptions _options;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;

            public TokenProviderMiddleware(
                RequestDelegate next,
                IOptions<TokenProviderOptions> options,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager
                )
            {
                _next = next;
                _options = options.Value;
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public Task Invoke(HttpContext context)
            {
                // If the request path doesn't match, skip
                if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
                {
                    return _next(context);
                }

                // Request must be POST with Content-Type: application/x-www-form-urlencoded
                if (!context.Request.Method.Equals("POST")
                   || !context.Request.HasFormContentType)
                {
                    context.Response.StatusCode = 400;
                    return context.Response.WriteAsync("Bad request.");
                }

                return GenerateToken(context);
            }
            private async Task GenerateToken(HttpContext context)
            {
                var username = context.Request.Form["username"];
                var password = context.Request.Form["password"];

                var identity = await GetIdentity(username, password);
                if (identity == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid username or password.");
                    return;
                }

                var now = DateTime.UtcNow;

                // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
                // You can add other claims here, if you want:
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                    new Claim(JwtRegisteredClaimNames.Iat, now.Ticks.ToString(), ClaimValueTypes.Integer64)
                };

                // Create the JWT and write it to a string
                var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(_options.Expiration),
                    signingCredentials: _options.SigningCredentials);
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)_options.Expiration.TotalSeconds
                };

                // Serialize and return the response
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
            private async Task<ClaimsIdentity> GetIdentity(string username, string password)
            {

                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    //TODO add lockout
                    var result = await _signInManager.PasswordSignInAsync(user,password,false,false);
                    if (result.Succeeded)
                    {
                        return new ClaimsIdentity(new System.Security.Principal.GenericIdentity(username, "Token"), new Claim[] { });
                    }
                    //TODO
                    else if (result.IsLockedOut) { }
                    else if (result.IsNotAllowed) { }
                    else if (result.RequiresTwoFactor) { }

                }

                // DON'T do this in production, obviously!
                if (username == "default" && password == "Asdf1234*")
                {
                    return new ClaimsIdentity(new System.Security.Principal.GenericIdentity(username, "Token"), new Claim[] { });
                }

                // Credentials are invalid, or account doesn't exist
                return null;
            }
        }
       
    }
}
