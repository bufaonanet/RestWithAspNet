using Microsoft.IdentityModel.JsonWebTokens;
using RestWithAspNet.Configurations;
using RestWithAspNet.Data.DTO;
using RestWithAspNet.Repository;
using RestWithAspNet.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestWithAspNet.Business
{
    public interface ILoginBusiness
    {
        public TokenDTO ValidateCredential(UserDTO user);
        public TokenDTO ValidateCredential(TokenDTO token);
        public bool RevokeToken(string username);
    }

    public class LoginBusiness : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private readonly TokenConfiguration _tokenConfiguration;
        private readonly IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public LoginBusiness(
            TokenConfiguration tokenConfiguration,
            IUserRepository repository,
            ITokenService tokenService)
        {
            _tokenConfiguration = tokenConfiguration;
            _repository = repository;
            _tokenService = tokenService;
        }

        public bool RevokeToken(string username)
        {
            return _repository.RevokeToken(username);
        }

        public TokenDTO ValidateCredential(UserDTO userCredentials)
        {
            var user = _repository.ValidateCredentials(userCredentials);

            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
            };

            var acessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_tokenConfiguration.DaysToExpiry);

            _repository.RefreshUserInfo(user);

            DateTime createdDate = DateTime.Now;
            DateTime expireDate = createdDate.AddMinutes(_tokenConfiguration.Minutes);

            return new TokenDTO(
                true,
                createdDate.ToString(DATE_FORMAT),
                expireDate.ToString(DATE_FORMAT),
                acessToken,
                refreshToken);

        }

        public TokenDTO ValidateCredential(TokenDTO token)
        {
            var acessToken = token.AcessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken);

            var userName = principal.Identity.Name;

            var user = _repository.ValidateCredentials(userName);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now) return null;

            acessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_tokenConfiguration.DaysToExpiry);

            _repository.RefreshUserInfo(user);

            DateTime createdDate = DateTime.Now;
            DateTime expireDate = createdDate.AddMinutes(_tokenConfiguration.Minutes);

            return new TokenDTO(
                true,
                createdDate.ToString(DATE_FORMAT),
                expireDate.ToString(DATE_FORMAT),
                acessToken,
                refreshToken);
        }
    }
}
