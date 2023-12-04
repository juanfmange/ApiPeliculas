using ApiPeliculas.Models;
using ApiPeliculas.Models.DTO;
using ApiPeliculas.Models.DTO.Peliculas;
using ApiPeliculas.Models.DTO.Users;
using ApiPeliculas.Models.Pelicula;
using ApiPeliculas.Models.Users;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper;

public class PeliculasMapper : Profile
{
    public PeliculasMapper()
    {
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        CreateMap<Categoria, CrearCategoriaDTO>().ReverseMap();
        CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        // CreateMap<Categoria, EditarCategoriaDTO>().ReverseMap();
        // CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
        // CreateMap<Pelicula, CrearPeliculaDTO>().ReverseMap();
        // CreateMap<Pelicula, EditarPeliculaDTO>().ReverseMap();
    }
}