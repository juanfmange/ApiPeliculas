using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.DTO.Peliculas;

public class PeliculaDTO
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Nombre { get; set; }
    
    public string RutaImagen { get; set; }
    
    [Required(ErrorMessage = "La descripción es requerida")]
    public string Descripcion { get; set; }
    
    [Required(ErrorMessage = "La duración es requerida")]
    public int Duracion { get; set; }
    
    public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
    
    public TipoClasificacion Clasificacion { get; set; }
    
    public DateTime FechaCreacion { get; set; }
    
    public int categoriaId { get; set; }
}
