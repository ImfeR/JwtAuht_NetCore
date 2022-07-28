namespace WebServiceAuthentication.Models.Auth;

using System.Text;
using Microsoft.IdentityModel.Tokens;

public class AuthOptions
{
    public const string Issuer = "SpttWebConfigurator";

    private const string Key = "webConfiguratorSecretKey_Af9021gasx";

    public const int Lifetime = 30;

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}