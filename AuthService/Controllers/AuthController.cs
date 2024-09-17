using AuthService.DTOs;
using AuthService.Models;
using AuthService.Services;
using CustonJwtAuthManager;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{

    [ApiController]
    [Route("api/[controller]/[Action]")]
    
    public class AuthController : ControllerBase
    {
       private readonly ApiResponseDTO _responseDTO=new ApiResponseDTO();
        private readonly IAuthService _authService;
        private readonly JwtTokenHandler _jwtToken;
        public AuthController(IAuthService authService, JwtTokenHandler jwtToken) { 
            _authService = authService;
            _jwtToken=jwtToken;
        }
        [HttpGet]
        public IActionResult IsWorking()
        {
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Ok("Working");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login) { 
        var response=await _authService.LoginUser(login);
        

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO Users)
        {
            var response=await _authService.RegisterUser(Users);
            return Ok(response);
        }
    }
}
