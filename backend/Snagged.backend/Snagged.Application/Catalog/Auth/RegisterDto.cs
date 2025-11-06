namespace Snagged.Application.Catalog.Auth
{
    public class RegisterDto
    {
        public string Email { get; set; }         // Login and register dtos are the same for now, but can differentiate later oniot
        public string Password { get; set; }
    }
}
