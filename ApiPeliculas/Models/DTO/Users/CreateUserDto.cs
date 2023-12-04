using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.DTO.Users;

public class CreateUserDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Nombre { get; set; }
    
    [Required(ErrorMessage = "El usuario es requerido")]
    public string NombreUsuario { get; set; }
    
    [Required(ErrorMessage = "El password es requerido")]
    public string Password { get; set; }
    
    public string Rol { get; set; }
}