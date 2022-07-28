namespace WebServiceAuthentication.Controllers;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Helpers;
using Services;
using Dto;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthorizationController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [Route("/Login")]
    [HttpPost]
    public IActionResult Login([FromBody] UserDto userDto)
    {
        User user;
        if (!_userService.TryGetUserByCredentials(userDto, out user))
            return BadRequest("Invalid login information");

        var jwtToken = JwtTokenHelper.GetJwtSecurityToken(user);
        var response = new LoginResponseDto { AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken) };

        return Ok(response);
    }

    [Route("")]
    [HttpPost]
    public IActionResult CreateUser()
    {
        _userService.CreateDefaultUser();

        return Ok();
    }
}