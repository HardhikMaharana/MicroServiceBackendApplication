using CustonJwtAuthManager.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CustonJwtAuthManager
{
    public  class JwtTokenHandler
    {
        public const string Jwt_Sercrity_Key = "_4m-ICEzabCSxibvDf5QxN7Dd6KzpWyHgiytzsZRhxYTnh8q92cRVsy1e4yKvTXGfjDDR1cDOfiSvbKQRo5cpw";
        public const int Jwt_Token_Validity = 20;

        public async Task<JwtResponseModel> JwtTokenGeneration(JwtRequestModel Req)
        {
            JwtResponseModel res=new JwtResponseModel();
            try
            {
                var AccessToken = await AccessTokenGeneration(Req);
                var RefreshToken = await GenerateRefreshtoken();
                if (AccessToken != null && RefreshToken!=null)
                {
                    res.AccessToken = AccessToken;
                    res.RefreshToken = RefreshToken;
                    return res;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async  Task<string> AccessTokenGeneration(JwtRequestModel Req)
        {
            try
            {
                var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt_Sercrity_Key));
                var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
                var Token_LifeTime = DateTime.Now.AddMinutes(Jwt_Token_Validity);

                var UserClaims = new[]
                {
                       new Claim(JwtRegisteredClaimNames.Name,Req.UserName??""),
                       new Claim(JwtRegisteredClaimNames.NameId,Req.Id??""),
                       new Claim(JwtRegisteredClaimNames.Email,Req.Email??""),
                       new Claim("Role",Req.Role??"")
                   };
                var Token = new JwtSecurityToken(
                claims: UserClaims,
                expires: Token_LifeTime,
                signingCredentials: credentials,
                issuer: "https://localhost:7164",
                audience: "https://localhost:7164"

                );

                var AccessToken = new JwtSecurityTokenHandler().WriteToken(Token);
                return AccessToken;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClaimsPrincipal> GetPrincipal(string AccessToken)
        {
            try
            {
                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt_Sercrity_Key));

                var validations = new TokenValidationParameters
                {
                    IssuerSigningKey = Key,
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false
                };
                return new JwtSecurityTokenHandler().ValidateToken(AccessToken, validations, out _);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public  async Task<JwtResponseModel> GetRefreshToken(JwtRefreshRequestModel jwtRefresh)
        {
            JwtResponseModel res=new JwtResponseModel();
            try
            {
                var tokenPrincipal =await GetPrincipal(jwtRefresh.Tokens.AccessToken);

                if (tokenPrincipal.Claims.FirstOrDefault().Value==null)
                {
                    return null;
                }
                else
                {
                    JwtRequestModel req = new JwtRequestModel {
                        Email = jwtRefresh.Email,
                        Id = jwtRefresh.Id,
                        Role = jwtRefresh.Role,
                        UserName = jwtRefresh.UserName,
                    };
                    var AccessToken = await AccessTokenGeneration(req);
                    var RefreshToken = await GenerateRefreshtoken();

                    if (AccessToken != null && RefreshToken != null) {
                        res.AccessToken = AccessToken;
                        res.RefreshToken= RefreshToken;

                        return res;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<string> GenerateRefreshtoken()
        {
            try
            {
                var reftoken = new byte[64];
                using (var randomnum = RandomNumberGenerator.Create())
                {
                    randomnum.GetBytes(reftoken);
                }
                return  Convert.ToBase64String(reftoken);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
