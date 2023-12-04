using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.DTO.Users;

public class UserLoginDto
{
    [Required(ErrorMessage = "El usuario es requerido")]
    public string NombreUsuario { get; set; }
    
    [Required(ErrorMessage = "El password es requerido")]
    public string Password { get; set; }
}