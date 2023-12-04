namespace ApiPeliculas.Models.DTO.Users;

public class UserDto
{
    public int Id { get; set; }
    
    public string Nombre { get; set; }
    
    public string NombreUsuario { get; set; }
    
    public string Password { get; set; }
    
    public string Rol { get; set; }
}