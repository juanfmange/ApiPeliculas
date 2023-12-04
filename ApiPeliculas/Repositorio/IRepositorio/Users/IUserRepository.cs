using ApiPeliculas.Models.DTO.Users;
using ApiPeliculas.Models.Users;

namespace ApiPeliculas.Repositorio.IRepositorio.Users;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    
    User GetUser(int userId);
    
    bool IsUniqueUser(string userName);
    
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    Task<User> CreateAccount (CreateUserDto createUserDto);
}