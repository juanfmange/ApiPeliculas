using System.Net;
using ApiPeliculas.Models;
using ApiPeliculas.Models.DTO.Users;
using ApiPeliculas.Repositorio.IRepositorio.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ApiPeliculas.Controllers;
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    protected ResponseApi _responseApi;
    private readonly IUserRepository _usRepo;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository usRepo, IMapper mapper)
    {
        _usRepo = usRepo;
        this._responseApi = new ResponseApi();
        _mapper = mapper;
    }    
    
    [HttpGet]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    
    [AllowAnonymous]
    [HttpPost("registro")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
    public async Task<IActionResult> CreateAccount([FromBody] CreateUserDto createUserDto)
    {
        bool validateUniqueUser = _usRepo.IsUniqueUser(createUserDto.NombreUsuario);
        
        if (!validateUniqueUser)
        {
            _responseApi.StatusCode = HttpStatusCode.BadRequest; 
            _responseApi.IsSuccess = false;
            _responseApi.ErrorMessages.Add("El usuario ya existe");
            return BadRequest(_responseApi);
        }

        var user = await _usRepo.CreateAccount(createUserDto);
        if (user == null)
        {
            _responseApi.StatusCode = HttpStatusCode.BadRequest;
            _responseApi.IsSuccess = false;
            _responseApi.ErrorMessages.Add("Error al crear el usuario");
            return BadRequest(_responseApi);
        }
        
        _responseApi.StatusCode = HttpStatusCode.OK;
        _responseApi.IsSuccess = true;
        return Ok(_responseApi);

    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var responseLogin = await _usRepo.Login(userLoginDto);
        
        if (responseLogin.User == null || string.IsNullOrEmpty(responseLogin.Token))
        {
            _responseApi.StatusCode = HttpStatusCode.BadRequest; 
            _responseApi.IsSuccess = false;
            _responseApi.ErrorMessages.Add("El usuario no existe o credenciales incorrectas");
            return BadRequest(_responseApi);
        }

        _responseApi.StatusCode = HttpStatusCode.OK;
        _responseApi.IsSuccess = true;
        _responseApi.Result = responseLogin;
        return Ok(_responseApi);
    }
    
}