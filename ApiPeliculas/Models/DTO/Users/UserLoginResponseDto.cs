using ApiPeliculas.Models.Users;

namespace ApiPeliculas.Models.DTO.Users;

public class UserLoginResponseDto
{
    public User User { get; set; }
    public string Token { get; set; }
    
}