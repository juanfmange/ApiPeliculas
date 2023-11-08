using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data;

public class ApplicationDbContext : DbContext
{
    //Instanciar y heredar las clases del consturctor para heredar
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    //Agrega los modelos
    public DbSet<Categoria> Categoria { get; set; }
}