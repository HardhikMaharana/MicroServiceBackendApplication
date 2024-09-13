using AuthService.DTOs;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class AuthService
    {
        private readonly ApiResponseDTO _apires;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _db;
        public AuthService(ApiResponseDTO apires, RoleManager<IdentityRole> roleManager, UserManager<Users> userManager, ApplicationDbContext db) {
            _apires = apires;
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        
        public async Task<ApiResponseDTO> RegisterUser(Users user)
        {
            try
            {
                var IsRolePresent = await _roleManager.FindByNameAsync("Admin");
                var isUser = await _userManager.FindByEmailAsync(user.Email);

                if (isUser == null) {
                    
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
    }
}
