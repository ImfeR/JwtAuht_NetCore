namespace WebServiceAuthentication.Services;

using Dto;
using Entities;

public interface IUserService
{
    bool TryGetUserByCredentials(UserDto credentials, out User user);
    void CreateDefaultUser();
}