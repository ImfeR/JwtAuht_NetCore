namespace WebServiceAuthentication.Dto;

using Newtonsoft.Json;

public class LoginResponseDto
{
    [JsonProperty("accessToken")] 
    public string AccessToken { get; set; }
}