using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.DTO;

public class CategoriaDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(60,ErrorMessage = "El numero maximo de caracteres es 60")]
    public string Nombre { get; set; }

}