using ApiPeliculas.Models.DTO.Users;
using ApiPeliculas.Repositorio.IRepositorio.Users;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ApiPeliculas.Controllers;
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _usRepo;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository usRepo, IMapper mapper)
    {
        _usRepo = usRepo;
        _mapper = mapper;
    }    
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetUsers()
    {
        var listaUsuarios = _usRepo.GetUsers();
                
        var listaUsuariosDTO = new List<UserDto>();

        foreach (var lista in listaUsuarios)
        {
            listaUsuariosDTO.Add(_mapper.Map<UserDto>(lista));
        }
        return Ok(listaUsuariosDTO);
    }
    
    
    [HttpGet("{userId:int}", Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUser(int userId)
    {
        var itemUser = _usRepo.GetUser(userId);

        if (itemUser == null)
        {
            return NotFound();
        }
                
        var itemUserDto = _mapper.Map<UserDto>(itemUser);

        return Ok(itemUserDto);
    }
}