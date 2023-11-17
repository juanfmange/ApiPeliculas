using ApiPeliculas.Models;
using ApiPeliculas.Models.DTO;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper;

public class PeliculasMapper : Profile
{
    public PeliculasMapper()
    {
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        CreateMap<Categoria, CrearCategoriaDTO>().ReverseMap();
        // CreateMap<Categoria, EditarCategoriaDTO>().ReverseMap();
        // CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
        // CreateMap<Pelicula, CrearPeliculaDTO>().ReverseMap();
        // CreateMap<Pelicula, EditarPeliculaDTO>().ReverseMap();
    }
}