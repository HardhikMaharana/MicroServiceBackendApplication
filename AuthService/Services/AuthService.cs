using AuthService.DTOs;
using AuthService.Models;
using CustonJwtAuthManager;
using CustonJwtAuthManager.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
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
        [HttpPost]
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
                        JwtRequestModel req = new JwtRequestModel
                        {
                              Id =IsValidEmail.Id,
                              Email=IsValidEmail.Email,
                              
                              UserName=IsValidEmail.UserName, 
                              
                        };
                        var token = _jwtToken.JwtTokenGeneration(req);
                        _apires.Tokens = token;
                        _apires.IsSuccessful = true;
                        _apires.Message = "Login Success";
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
    }
}
