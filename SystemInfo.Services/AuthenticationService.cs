using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Services {
    public interface IAuthenticationService {
        OperationResponse<TokenResponse> GetToken(TokenRequest tokenRequest);
    }
    public class AuthenticationService : BaseService, IAuthenticationService {
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public OperationResponse<TokenResponse> GetToken(TokenRequest tokenRequest) {
            if (_configuration["ApiPassword"] != tokenRequest.Password) {
                return Error("Contraseña invalida" , new TokenResponse() , ServiceResult.InvalidData);
            }

            var claims = new[] {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:SecretKey").Value));
            var credencials = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);
            int expirationMinutes = Convert.ToInt32(_configuration["Jwt:TokenExpirationMinutes"] ?? "15");
            var expirationDate = DateTime.UtcNow.AddMinutes(expirationMinutes);

            JwtSecurityToken token = new JwtSecurityToken(
                   issuer: "sag.com" ,
                   audience: "sag.com" ,
                   claims: claims ,
                   expires: expirationDate ,
                   signingCredentials: credencials
               );

            return new OperationResponse<TokenResponse>() {
                Message = "Login exitoso" ,
                OperationResult = ServiceResult.Success ,
                Record = new TokenResponse {
                    Token = new JwtSecurityTokenHandler().WriteToken(token) ,
                    ExpirationDate = expirationDate
                }
            };

        }
    }


}
