namespace WebServiceAuthentication.Dto;

using Newtonsoft.Json;
public class UserDto
{
    [JsonProperty("username")] 
    public string Username { get; set; }


    [JsonProperty("password")] 
    public string Password { get; set; }
}