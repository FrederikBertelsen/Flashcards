using Backend.Exceptions;
using Backend.Mappers;
using Backend.Models;
using Backend.Models.DbContext;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UsersRepository(AppDbContext dbContext)
{
    public async Task<UserDTO> CreateUser(NewUserDTO newUserDto)
    {
        await ValidateUserName(newUserDto.Name);

        if (string.IsNullOrWhiteSpace(newUserDto.GoogleId))
            throw new ArgumentException("GoogleId cannot be empty.");
        if (await dbContext.Users.AnyAsync(u => u.GoogleId == newUserDto.GoogleId))
            throw new ModelAlreadyExistsException("User with this GoogleId already exists.");

        var newUser = newUserDto.ToEntity();

        var addUser = (await dbContext.Users.AddAsync(newUser)).Entity;

        await dbContext.SaveChangesAsync();

        return addUser.ToDto();
    }

    public async Task<UserDTO> GetUserById(string userId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        if (user == null)
            throw new ModelNotFoundException($"User with id {userId} not found");

        return user.ToDto();
    }

    public async Task<UserDTO> GetUserByGoogleId(string googleId)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);
        if (user == null)
            throw new ModelNotFoundException($"User with Google Id {googleId} not found");

        return user.ToDto();
    }

    public async Task<bool> IsUserAdmin(string userId, string googleId, User user)
    {
        var foundUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId && u.GoogleId == googleId);
        if (foundUser == null)
            throw new ModelNotFoundException($"User with id {userId} and Google id {googleId} not found");

        if (foundUser.Id != user.Id || foundUser.GoogleId != user.GoogleId)
            throw new ArgumentException("The provided user credentials do not match the target user.");

        return foundUser.IsAdmin;
    }

    public async Task ChangeName(string userId, string newUsername, User user)
    {
        var userToChange = await dbContext.Users.FindAsync(userId);
        if (userToChange == null)
            throw new ModelNotFoundException($"User with id {userId} not found.");

        if (userToChange.Id != user.Id || userToChange.GoogleId != user.GoogleId)
            throw new ArgumentException("The provided user credentials do not match the target user.");

        await ValidateUserName(newUsername);

        userToChange.Name = newUsername;
        dbContext.Users.Update(userToChange);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(string userId, User user)
    {
        var userToBeDeleted = await dbContext.Users.FindAsync(userId);
        if (userToBeDeleted == null)
            throw new ModelNotFoundException($"User with id {userId} not found");
        if (userToBeDeleted.IsAdmin)
            throw new ArgumentException("Admin user cannot be deleted.");

        if (userToBeDeleted.Id != user.Id || userToBeDeleted.GoogleId != user.GoogleId)
            throw new ArgumentException("The provided user credentials do not match the target user.");

        dbContext.Users.Remove(userToBeDeleted);
        await dbContext.SaveChangesAsync();
    }

    private static async Task ValidateUserName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Username cannot be empty.");
        if (name.Length < 3)
            throw new ArgumentException($"Username must be at least {Constants.MinUsernameLength} characters long.");
        if (name.Length > Constants.MaxNameLength)
            throw new ArgumentException($"Username must be at most {Constants.MaxNameLength} characters long.");
        // if (await dbContext.Users.AnyAsync(u => u.Name == name))
        //     throw new ModelAlreadyExistsException("User with this name already exists.");
    }
}