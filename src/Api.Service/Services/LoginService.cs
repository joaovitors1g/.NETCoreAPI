using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Domain.DTOs;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service.Services
{
   public class LoginService : ILoginService
   {
      private readonly IUserRepository _repository;

      private readonly SigningConfiguration _signingConfigurations;

      private readonly TokenConfiguration _tokenConfigurations;

      private IConfiguration _configuration { get; }

      public LoginService(IUserRepository repository, SigningConfiguration signingConfigurations, TokenConfiguration tokenConfigurations, IConfiguration configuration)
      {
         _repository = repository;
         _signingConfigurations = signingConfigurations;
         _tokenConfigurations = tokenConfigurations;
         _configuration = configuration;
      }

      public async Task<object> FindByLogin(LoginDTO user)
      {
         var baseUser = new UserEntity();

         if (user != null && !string.IsNullOrWhiteSpace(user.Email))
         {
            baseUser = await _repository.FindByLogin(user.Email);

            if (baseUser == null)
            {
               return new
               {
                  authenticated = false,
                  message = "Falha ao autenticar"
               };
            }

            var identity = new ClaimsIdentity
            (
               new GenericIdentity(baseUser.Email),
               new[]
               {
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.UniqueName, baseUser.Email)
               }
            );

            var creationDate = DateTime.Now;

            var expirationDate = creationDate + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();

            string token = CreateToken(identity, creationDate, expirationDate, handler);

            return SuccessObject(creationDate, expirationDate, token, user);
         }
         else
         {
               return new
               {
                  authenticated = false,
                  message = "Falha ao autenticar"
               };
         }
      }

      private string CreateToken(ClaimsIdentity identity, DateTime creationDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
      {
         var securityToken = handler.CreateToken(new SecurityTokenDescriptor
         {
            Issuer = _tokenConfigurations.Issuer,
            Audience = _tokenConfigurations.Audience,
            SigningCredentials = _signingConfigurations.SigningCredentials,
            Subject = identity,
            NotBefore = creationDate,
            Expires = expirationDate
         });

         var token = handler.WriteToken(securityToken);

         return token;
      }

      private object SuccessObject(DateTime creationDate, DateTime expirationDate, string token, LoginDTO user)
      {
         return new
         {
            authenticated = true,
            created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
            expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
            accessToken = token,
            username = user.Email,
            message = "Usuário logado com sucesso!"
         };
      }
   }
}
