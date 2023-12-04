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

    public Task<User> CreateAccouny(CreateUserDto createUserDto)
    {
        return _userRepositoryImplementation.CreateAccouny(createUserDto);
    }
}