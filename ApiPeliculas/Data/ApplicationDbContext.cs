using ApiPeliculas.Models;
using ApiPeliculas.Models.Pelicula;
using ApiPeliculas.Models.Users;
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
    public DbSet<Pelicula> Pelicula { get; set; }
    
    public DbSet<User> User { get; set; }
}