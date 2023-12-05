using ApiPeliculas.Models;
using ApiPeliculas.Models.DTO;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
                _ctRepo = ctRepo;
                _mapper = mapper;
        }
        
        [AllowAnonymous]
        [HttpGet]
        //[ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any)]
        [ResponseCache(CacheProfileName = "Default20")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
                var listaCategorias = _ctRepo.GetCategorias();
                
                var listaCategoriasDTO = new List<CategoriaDTO>();

                foreach (var lista in listaCategorias)
                {
                        listaCategoriasDTO.Add(_mapper.Map<CategoriaDTO>(lista));
                }
                return Ok(listaCategoriasDTO);
        }
        
        [AllowAnonymous]
        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
                var itemCategoria = _ctRepo.GetCategoria(categoriaId);

                if (itemCategoria == null)
                {
                        return NotFound();
                }
                
                var itemCategoriaDTO = _mapper.Map<CategoriaDTO>(itemCategoria);

                return Ok(itemCategoriaDTO);
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201,Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDTO crearCategoriaDto)
        {
                //var itemCategoria = _ctRepo.GetCategoria(categoriaId);

                if (!ModelState.IsValid)
                {
                        return BadRequest();
                }

                if (crearCategoriaDto == null)
                {
                        return BadRequest(ModelState);
                }
                if(_ctRepo.ExisteCategoria(crearCategoriaDto.Nombre))
                {
                        ModelState.AddModelError("", "La categoria ya existe");
                        return StatusCode(404, ModelState);
                }
                
                
                var categoria = _mapper.Map<Categoria>(crearCategoriaDto);
                if(!_ctRepo.CrearCategoria(categoria))
                {
                        ModelState.AddModelError("", $"Algo salio mal guardando el registro {categoria.Nombre}");
                        return StatusCode(500, ModelState);
                }
                
                return CreatedAtRoute("GetCategoria", new {categoriaId = categoria.Id}, categoria);
        }
        
        //Actualizar patch categoria
        [Authorize(Roles = "admin")]
        [HttpPatch("{categoriaId:int}", Name = "ActualizarCategoria")]
        [ProducesResponseType(201,Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CategoriaDTO categoriaDto)
        {

                if (!ModelState.IsValid)
                {
                        return BadRequest();
                }

                if (categoriaDto == null || categoriaId != categoriaDto.Id)
                {
                        return BadRequest(ModelState);
                }
                
                
                
                
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                if(!_ctRepo.ActualizarCategoria(categoria))
                {
                        ModelState.AddModelError("", $"Algo salio mal actualizando el registro {categoria.Nombre}");
                        return StatusCode(500, ModelState);
                }

                return NoContent();
        }
        
        //Borrar categoria
        [Authorize(Roles = "admin")]
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        
        public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CrearCategoriaDTO categoriaDto)
        {
                if (!_ctRepo.ExisteCategoria(categoriaId))
                {
                        return NotFound();
                }

                var categoria = _ctRepo.GetCategoria(categoriaId);
                if(!_ctRepo.BorrarCategoria(categoria))
                {
                        ModelState.AddModelError("", $"Algo salio mal borrando el registro {categoria.Nombre}");
                        return StatusCode(500, ModelState);
                }

                return NoContent();
        }
        
        
}