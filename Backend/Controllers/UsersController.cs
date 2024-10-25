using Backend.Exceptions;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(UsersRepository usersRepository, SessionManager sessionManager)
    : BaseController(sessionManager)
{
    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser(NewUserDTO newUserDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var userDto = await usersRepository.CreateUser(newUserDto);
            return Ok(userDto);
        });
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDTO>> GetUserById(string userId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var userDto = await usersRepository.GetUserById(userId);
            return Ok(userDto);
        });
    }

    [HttpGet("google/{googleId}")]
    public async Task<ActionResult<UserDTO>> GetUserByGoogleId(string googleId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var userDto = await usersRepository.GetUserByGoogleId(googleId);
            return Ok(userDto);
        });
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult> ChangeName(string userId, string newUsername)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await usersRepository.ChangeName(userId, newUsername, user);
                return NoContent();
            });
        });
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await usersRepository.DeleteUser(userId, user);
                return NoContent();
            });
        });
    }

    [HttpPost("admin")]
    public async Task<ActionResult<bool>> IsUserAdmin(string userId, string googleId)
    {
        return await ExceptionHandler.HandleAsync(async () => { 
            return await WithAuthAsync(async user =>
            {
                var isAdmin = await usersRepository.IsUserAdmin(userId, googleId, user);
                return Ok(isAdmin);
            });
        });
    }
}