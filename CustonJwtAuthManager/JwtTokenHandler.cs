using CustonJwtAuthManager.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CustonJwtAuthManager
{
    public  class JwtTokenHandler
    {
        public const string Jwt_Sercrity_Key = "_4m-ICEzabCSxibvDf5QxN7Dd6KzpWyHgiytzsZRhxYTnh8q92cRVsy1e4yKvTXGfjDDR1cDOfiSvbKQRo5cpw";
        public const int Jwt_Token_Validity = 20;

        public JwtResponseModel JwtTokenGeneration(JwtRequestModel Req)
        {
            JwtResponseModel res=new JwtResponseModel();
            try
            {
               var SecurityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt_Sercrity_Key));
                var credentials=new SigningCredentials(SecurityKey,SecurityAlgorithms.HmacSha256);
                var Token_LifeTime=DateTime.Now.AddMinutes(Jwt_Token_Validity);

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
                if (AccessToken != null)
                {
                    res.AccessToken = AccessToken;
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

       
    }
}
