using ApiPeliculas.Models.DTO.Peliculas;
using ApiPeliculas.Models.Pelicula;
using ApiPeliculas.Repositorio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;


namespace ApiPeliculas.Controllers;
[Route("api/peliculas")]
[ApiController]


public class PeliculasController : ControllerBase
{
    private readonly IPeliculaRepository _peliculaRepository;
    private readonly IMapper _mapper;
    
    public PeliculasController(IPeliculaRepository peliculaRepository, IMapper mapper)
    {
        _peliculaRepository = peliculaRepository;
        _mapper = mapper;
    }
    
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetPeliculas()
    {
        var listaPeliculas = _peliculaRepository.GetPeliculas();
                
        var listaPeliculasDTO = new List<PeliculaDTO>();

        foreach (var lista in listaPeliculas)
        {
            listaPeliculasDTO.Add(_mapper.Map<PeliculaDTO>(lista));
        }
        return Ok(listaPeliculasDTO);
    }
    
    [AllowAnonymous]
    [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPelicula(int peliculaId)
    {
        var itemPelicula = _peliculaRepository.GetPelicula(peliculaId);

        if (itemPelicula == null)
        {
            return NotFound();
        }
                
        var itemPeliculaDTO = _mapper.Map<PeliculaDTO>(itemPelicula);

        return Ok(itemPeliculaDTO);
    }
    
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(201,Type = typeof(PeliculaDTO))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
    public IActionResult CrearPelicula([FromBody] PeliculaDTO peliculaDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (peliculaDto == null)
        {
            return BadRequest(ModelState);
        }
        if(_peliculaRepository.ExistePelicula(peliculaDto.Nombre))
        {
            ModelState.AddModelError("", "La pelicula ya existe");
            return StatusCode(404, ModelState);
        }
                
                
        var pelicula = _mapper.Map<Pelicula>(peliculaDto);
        if(!_peliculaRepository.CrearPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salio mal guardando el registro {pelicula.Nombre}");
            return StatusCode(500, ModelState);
        }
                
        return CreatedAtRoute("GetPelicula", new {peliculaId = pelicula.Id}, pelicula);
    }
    
    
    
    [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(201,Type = typeof(PeliculaDTO))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
    public IActionResult ActualizarPelicula(int peliculaId, [FromBody] PeliculaDTO peliculaDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (peliculaDto == null || peliculaId != peliculaDto.Id)
        {
            return BadRequest(ModelState);
        }
                
                
                
                
        var pelicula = _mapper.Map<Pelicula>(peliculaDto);
        if(!_peliculaRepository.ActualizarPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salio mal actualizando el registro {pelicula.Nombre}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    
    
    
    
    //Borrar pelicula
    //Borrar categoria
    [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        
    public IActionResult BorrarPelicula(int peliculaId)
    {
        if (!_peliculaRepository.ExistePelicula(peliculaId))
        {
            return NotFound();
        }

        var pelicula = _peliculaRepository.GetPelicula(peliculaId);
        if(!_peliculaRepository.BorrarPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salio mal borrando el registro {pelicula.Nombre}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    
    //Peliculas en categoria
    [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
    public IActionResult GetPeliculasEnCategoria(int categoriaId)
    {
        var listaPeliculas = _peliculaRepository.GetPeliculasEnCategoria(categoriaId);
        if (listaPeliculas == null)
        {
            return NotFound();
        }

        var itemPelicula = new List<PeliculaDTO>();
        foreach (var lista in listaPeliculas)
        {
            itemPelicula.Add(_mapper.Map<PeliculaDTO>(lista));
        }

        return Ok(itemPelicula);
    }
    
    
    //Buscar pelicula
    [HttpGet("Buscar")]
    public IActionResult Buscar(string nombre)
    {

        try
        {
            var resultado = _peliculaRepository.BuscarPelicula(nombre.Trim());
            if (resultado.Any())
            {
                return Ok(resultado);
            }

            return NotFound();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicacion");
        }
    }
    
    
}