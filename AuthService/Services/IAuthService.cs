using AuthService.DTOs;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<ApiResponseDTO> LoginUser(LoginDTO login);
        Task<ApiResponseDTO> RegisterUser(UserRegisterDTO user);
    }
}