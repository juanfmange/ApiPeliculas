using ApiPeliculas.Models;

namespace ApiPeliculas.Repositorio.IRepositorio;

public interface ICategoriaRepositorio
{
    ICollection<Categoria> GetCategorias();
    
    Categoria GetCategoria(int CategorId);
    
    bool ExisteCategoria(string nombre);
    
    bool ExisteCategoria(int id);
    
    bool CrearCategoria(Categoria categoria);
    
    bool ActualizarCategoria(Categoria categoria);
    
    bool BorrarCategoria(Categoria categoria);
    
    bool Guardar();
    
    
}