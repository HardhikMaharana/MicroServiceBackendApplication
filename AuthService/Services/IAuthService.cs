using AuthService.DTOs;
using CustonJwtAuthManager.Model;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<ApiResponseDTO> LoginUser(LoginDTO login);
        Task<ApiResponseDTO> RegisterUser(UserRegisterDTO user);
        Task<ApiResponseDTO> GetJwtRefreshToken(JwtRefreshRequestModel req);
    }
}