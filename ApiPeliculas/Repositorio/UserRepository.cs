using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiPeliculas.Data;
using ApiPeliculas.Models.DTO.Users;
using ApiPeliculas.Models.Users;
using ApiPeliculas.Repositorio.IRepositorio.Users;
using Microsoft.IdentityModel.Tokens;

namespace ApiPeliculas.Repositorio;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _bd;
    private string secretKey;
    
    //private IUserRepository _userRepositoryImplementation;

    public UserRepository(ApplicationDbContext bd, IConfiguration config)
    {
        _bd = bd;
        secretKey = config.GetValue<string>("ApiSettings:Secret");
    }


    public ICollection<User> GetUsers()
    {
        return _bd.User.OrderBy(u => u.Nombre).ToList();
    }

    public User GetUser(int userId)
    {
        return _bd.User.FirstOrDefault(u => u.Id == userId);
    }

    public bool IsUniqueUser(string userName)
    {
        var userbd = _bd.User.FirstOrDefault(u => u.NombreUsuario == userName);
        if(userbd == null)
        {
            return true;
        }
        return false;
    }

    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        var passwordEncrypted = encrypted(userLoginDto.Password);

        var user = _bd.User.FirstOrDefault(
            u => u.NombreUsuario.ToLower() == userLoginDto.NombreUsuario.ToLower()
                 && u.Password == passwordEncrypted
        );
        
        
        //Validamos si el usuario no existe con la combinacion de usuario y password correcto
        if(user == null)
        {
            return new UserLoginResponseDto()
            {
                Token = "",
                User = null
            };
        }
        
        //Si el usuario existe, generamos el token
        var tokenManager = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        { 
            Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, user.Rol)
                }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenManager.CreateToken(tokenDescriptor);

        UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto()
        {
            Token = tokenManager.WriteToken(token),
            User = user
        };

        return userLoginResponseDto;
    }

    public async Task<User> CreateAccount(CreateUserDto createUserDto)
    {
        var passwordEncrypted = encrypted(createUserDto.Password);
        User user = new User()
        {
            NombreUsuario = createUserDto.NombreUsuario,
            Password = passwordEncrypted,
            Nombre = createUserDto.Nombre,
            Rol = createUserDto.Rol
        };
        
        _bd.User.Add(user);
        
        await _bd.SaveChangesAsync();

        user.Password = passwordEncrypted;

        return user;
    }
    
    //Metodo para encryptar el password
    public static string encrypted(string valor)
    {
        MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        data = x.ComputeHash(data);
        string resp = "";
        for (int i = 0; i < data.Length; i++)
            resp += data[i].ToString("x2").ToLower();
        return resp;
    }
}