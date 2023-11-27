using ApiPeliculas.Repositorio.Interfaces;
using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Pelicula;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio;

public class PeliculaRepository : IPeliculaRepository
{
    private readonly ApplicationDbContext _contextPeliculaBd;
    
    public PeliculaRepository(ApplicationDbContext contextPeliculaBd)
    {
        _contextPeliculaBd = contextPeliculaBd;
    }
    
    //Obtiene las categorias
    public ICollection<Pelicula> GetPeliculas()
    {
        return _contextPeliculaBd.Pelicula.OrderBy(c => c.Nombre).ToList();
    }

    public Pelicula GetPelicula(int peliculaId)
    {
        return _contextPeliculaBd.Pelicula.FirstOrDefault(c => c.Id == peliculaId);
    }

    public bool ExistePelicula(string nombre)
    {
        bool valor = _contextPeliculaBd.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        return valor;
    }

    public bool ExistePelicula(int id)
    {
        return _contextPeliculaBd.Pelicula.Any(c => c.Id == id);
    }

    public bool CrearPelicula(Pelicula pelicula)
    {
        pelicula.FechaCreacion = DateTime.Now;
        _contextPeliculaBd.Pelicula.Add(pelicula);
        return Guardar();
    }

    public bool ActualizarPelicula(Pelicula pelicula)
    {
        pelicula.FechaCreacion = DateTime.Now;
        _contextPeliculaBd.Pelicula.Update(pelicula);
        return Guardar();
    }

    public bool BorrarPelicula(Pelicula pelicula)
    {
        _contextPeliculaBd.Pelicula.Remove(pelicula);
        return Guardar();
    }

    public ICollection<Pelicula> GetPeliculasEnCategoria(int catId)
    {
        return _contextPeliculaBd.Pelicula.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == catId).ToList();
    }

    public ICollection<Pelicula> BuscarPelicula(string nombre)
    {
        IQueryable < Pelicula > query = _contextPeliculaBd.Pelicula;

        if (!string.IsNullOrEmpty(nombre))
        {
            query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            
        }

        return query.ToList();
    }

    public bool Guardar()
    {
        return _contextPeliculaBd.SaveChanges() >= 0 ? true : false;
    }
    
}