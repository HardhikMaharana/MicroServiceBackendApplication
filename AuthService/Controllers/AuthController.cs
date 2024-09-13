using AuthService.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
    {
       private readonly ApiResponseDTO _responseDTO;
        public AuthController(ApiResponseDTO apiResponseDTO) { 
        _responseDTO = apiResponseDTO;
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
    }
}
