
namespace SharedLibrary.Models
{
    public class LoginResponse
    {
        public string? TokenType { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
