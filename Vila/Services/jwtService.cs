using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Vila.Services
{
    // אבטחה, בדיקת הרשאות, 
    public class JwtService
    {
        /*
        private static string JwtSecretSign = "ProEMLh5e_qnzdNUQrqdHPgp";
        private static string JwtSecretDecrypt = "ProEMLh5e_qnzdNU";
        private TimeSpan TokenNumMinutesToExtend = new TimeSpan(1, 1, 0);
        private TimeSpan TokenMaxMinutesSession = new TimeSpan(2, 0, 0);

        public const string TokenPrimaryKey = "UserId";
        public static SymmetricSecurityKey JwtSymmetricSecurityIssuerSigningKey => new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(JwtSecretSign));
        public static SymmetricSecurityKey JwtSymmetricSecurityTokenDecryptionKey => new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(JwtSecretDecrypt));

        public IHttpContextAccessor HttpContextAccessor { get; }
        public HttpRequest Request => HttpContextAccessor.HttpContext?.Request;

        public JwtService(IHttpContextAccessor http)
        {
            HttpContextAccessor = http;
        }
        //החלק שאוכף את הטוקן והוא מוגדר בstartup.cs
        public static TokenValidationParameters TokenValidationParameters => new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = JwtSymmetricSecurityIssuerSigningKey,
            TokenDecryptionKey = JwtSymmetricSecurityTokenDecryptionKey,
            ValidIssuer = "issuer",
            ValidAudience = "Audience",
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero

        };

        //החלק שמחולל את המפתח -הטוקן
        public string GenerateToken(string tokenPrimaryValue, bool Admin)
        {
            DateTime expired = DateTime.Now.Add(TokenNumMinutesToExtend);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "issuer",
                Audience = "Audience",
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(TokenPrimaryKey, tokenPrimaryValue),
                    new Claim(ClaimTypes.Role, Admin ?  "Admin" : "BaseUser")
                }),
                Expires = expired,
                SigningCredentials = new SigningCredentials(JwtSymmetricSecurityIssuerSigningKey, SecurityAlgorithms.HmacSha512),
                EncryptingCredentials = new EncryptingCredentials(JwtSymmetricSecurityTokenDecryptionKey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenStr = $"Bearer {tokenHandler.WriteToken(token)}";

            return tokenStr;
        }

        // בודק מפתח הצפנה
        public string GetUserRole()
        {
            string tokenString = Request.Headers["Authorization"];
            tokenString ??= "";
            tokenString = tokenString.Trim();

            if (string.IsNullOrEmpty(tokenString)) { throw new Exception("JwtService - Token Is Empty"); }
            if (tokenString.Length <= "Bearer ".Length) { throw new Exception("JwtService - Token Is Too Short"); }

            try
            {
                var accesToken = tokenString.Substring("Bearer ".Length);
                // משתמשים בפונקציה של JWT בכדי להוציא את הפרמטרים של המפתח
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(accesToken, TokenValidationParameters, out var securityToken);
                JwtSecurityToken jwtToken = ((JwtSecurityToken)securityToken);

                return jwtToken.Claims.ToList().FirstOrDefault(claim => claim.Type == "role")?.Value;
            }
            catch (Exception ex)
            {
                throw new Exception($"JwtService Error TokenClaims  Invalid Token - {ex.Message}");
            }
        }

        public string GetTokenClaims()
        {
            string tokenString = Request.Headers["Authorization"];
            tokenString ??= "";
            tokenString = tokenString.Trim();

            if (string.IsNullOrEmpty(tokenString)) { throw new Exception("JwtService - Token Is Empty"); }
            if (tokenString.Length <= "Bearer ".Length) { throw new Exception("JwtService - Token Is Too Short"); }

            try
            {
                var accesToken = tokenString.Substring("Bearer ".Length);

                // משתמשים בפונקציה של JWT בכדי להוציא את הפרמטרים של המפתח
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(accesToken, TokenValidationParameters, out var securityToken);
                JwtSecurityToken jwtToken = ((JwtSecurityToken)securityToken);

                return jwtToken.Claims.ToList().FirstOrDefault(claim => claim.Type == TokenPrimaryKey)?.Value;
            }
            catch (Exception ex)
            {
                throw new Exception($"JwtService Error TokenClaims  Invalid Token - {ex.Message}");
            }
        }
        */
    }
}