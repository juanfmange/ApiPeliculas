using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Users;

public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Nombre { get; set; }
    
    public string NombreUsuario { get; set; }
    
    public string Password { get; set; }
    
    public string Rol { get; set; }
    
    public DateTime FechaCreacion { get; set; }
    
}