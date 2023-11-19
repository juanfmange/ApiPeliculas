using ApiPeliculas.Models;
using ApiPeliculas.Models.DTO;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
                var listaCategorias = _ctRepo.GetCategorias();
                
                var listaCategoriasDTO = new List<CategoriaDTO>();

                foreach (var lista in listaCategoriasDTO)
                {
                        listaCategoriasDTO.Add(_mapper.Map<CategoriaDTO>(lista));
                }
                return Ok(listaCategoriasDTO);
        }
        
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
        
        [HttpPost]
        [ProducesResponseType(201,Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        
        
}