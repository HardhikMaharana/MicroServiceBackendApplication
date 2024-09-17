using AuthService.DTOs;
using AuthService.Models;
using CustonJwtAuthManager;
using CustonJwtAuthManager.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApiResponseDTO _apires;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly JwtTokenHandler _jwtToken;
        public AuthService(ApiResponseDTO apires, RoleManager<IdentityRole> roleManager, UserManager<Users> userManager, ApplicationDbContext db, JwtTokenHandler jwtToken)
        {
            _apires = apires;
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _jwtToken = jwtToken;
        }
        public async Task<string> GetUserRolesAsync(string userId)
        {
            var userRoles = await (from ur in _db.UserRoles
                                   join r in _db.Roles on ur.RoleId equals r.Id
                                   where ur.UserId == userId
                                   select r.Name).FirstOrDefaultAsync();

            return userRoles;
        }
        public async Task<ApiResponseDTO> RegisterUser(UserRegisterDTO user)
        {
            try
            {
                var IsRolePresent = await _roleManager.FindByNameAsync("Admin");
                var isUser = await _userManager.FindByEmailAsync(user.Email);
                var identityUser = new Users
                {
                    UserName = user.UserName,
                    Email = user.Email,

                };
                if (isUser == null && IsRolePresent == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                    await _userManager.CreateAsync(identityUser, user.Password);
                    await _userManager.AddToRoleAsync(identityUser, "Admin");
                    _apires.IsSuccessful = true;
                    _apires.Message = "Admin Registered Successfully";
                }
                else
                {
                    await _userManager.CreateAsync(identityUser, user.Password);
                    await _userManager.AddToRoleAsync(identityUser, "Admin");
                    _apires.IsSuccessful = true;
                    _apires.Message = "Admin Registered Successfully";
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _apires;
        }
        public async Task<ApiResponseDTO> LoginUser(LoginDTO login)
        {
            try
            {
                var IsValidEmail = await _userManager.FindByEmailAsync(login.Email);

                if (IsValidEmail != null)
                {
                    var IsValidUser =await _userManager.CheckPasswordAsync(IsValidEmail, login.Password);

                    if (IsValidUser != false)
                    {
                        var userRole = await GetUserRolesAsync(IsValidEmail.Id);
                        JwtRequestModel req = new JwtRequestModel
                        {
                            Id = IsValidEmail.Id,
                            Email = IsValidEmail.Email,
                            Role = userRole,
                              UserName = IsValidEmail.UserName,

                        };
                        var token = await _jwtToken.JwtTokenGeneration(req);

                        if (token != null) {
                          
                            IsValidEmail.RefteshToken = token.RefreshToken;
                            IsValidEmail.RefreshTokenExpiry = DateTime.Now.AddHours(1);
                           var isupdated=await _userManager.UpdateAsync(IsValidEmail);
                            if (isupdated != null)
                            {
                                _apires.Tokens = token;
                                _apires.IsSuccessful = true;
                                _apires.Message = "Login Success";
                            }
                            else
                            {
                                _apires.IsSuccessful = false;
                                _apires.Message = "Something Went Wrong";
                            }
                          
                        }
                        else
                        {
                            _apires.IsSuccessful = false;
                            _apires.Message = "Something Went Wrong";
                        }
                
                    }
                    else
                    {
                        _apires.IsSuccessful = false;
                        _apires.Message = "Please Enter Valid Password";
                    }
                }
                else
                {
                    _apires.IsSuccessful = false;
                    _apires.Message = "Please Enter Valid Email";
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _apires;
        }
   
        public async Task<ApiResponseDTO> GetJwtRefreshToken(JwtRefreshRequestModel req)
        {
            try
            {
                var IsUserPresent=await _userManager.FindByEmailAsync(req.Email);
                if (IsUserPresent != null && req.Tokens.RefreshToken==IsUserPresent.RefteshToken && IsUserPresent.RefreshTokenExpiry>DateTime.Now) {
                    var userRole =  await GetUserRolesAsync(IsUserPresent.Id);
                    req.UserName = IsUserPresent.UserName??"";
                    req.Id = IsUserPresent.Id;
                    req.Email = IsUserPresent.Email ?? "";
                    req.Role = userRole.ToString();
                    req.Tokens.RefreshToken = IsUserPresent.RefteshToken;

                    var AuthRefreshToken = await _jwtToken.GetRefreshToken(req);
                    if (AuthRefreshToken != null) {
                        _apires.IsSuccessful=true;
                        _apires.Message = "Refreshed SuccessFully";
                        _apires.Tokens = AuthRefreshToken;
                    }
                    else
                    {
                        _apires.IsSuccessful = false;
                        _apires.Message = "Unauthorized Request Please Login";
                        _apires.Tokens = null;
                    }

                }
                else
                {
                    _apires.Message = "Session Expired Please Login";
                    _apires.IsSuccessful = false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _apires;
        }
    }
}
