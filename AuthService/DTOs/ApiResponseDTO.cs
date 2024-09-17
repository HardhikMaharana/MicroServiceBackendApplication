namespace AuthService.DTOs
{
    public class ApiResponseDTO
    {
        public bool IsSuccessful { get; set; }
        public object Data { get; set; } = new object();
        public string Message { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public object Tokens { get; set; } = new object();
    }
}
