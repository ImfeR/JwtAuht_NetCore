namespace WebServiceAuthentication.Services;

using Context;
using Dto;
using Entities;

public class UserService : IUserService
{
    private readonly WebConfiguratorContext _dbContext;

    public UserService(WebConfiguratorContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool TryGetUserByCredentials(UserDto credentials, out User user)
    {
        var usr = _dbContext.Users.FirstOrDefault(u => u.Username == credentials.Username);

        if (usr == null || usr.Password != credentials.Password)
        {
            user = new User();
            return false;
        }

        user = usr;
        return true;
    }

    public void CreateDefaultUser()
    {
        var user = new User
        {
            Password = "Admin",
            Username = "Admin",
            Role = "Administrator"
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }
}