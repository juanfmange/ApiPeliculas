using System.Security.Cryptography;
using ApiPeliculas.Data;
using ApiPeliculas.Models.DTO.Users;
using ApiPeliculas.Models.Users;
using ApiPeliculas.Repositorio.IRepositorio.Users;

namespace ApiPeliculas.Repositorio;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _bd;
    private IUserRepository _userRepositoryImplementation;

    public UserRepository(ApplicationDbContext bd)
    {
        _bd = bd;
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

    public Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        return _userRepositoryImplementation.Login(userLoginDto);
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