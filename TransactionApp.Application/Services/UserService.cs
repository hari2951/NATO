using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Interfaces;
using TransactionApp.Application.Utilities;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Interfaces;
using TransactionApp.Infrastructure.Data;

namespace TransactionApp.Application.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        : IUserService
    {
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            try
            {
                logger.LogInformation(LogMessages.FetchingAllUsers);
                var users = await userRepository.GetAllAsync();
                return mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch all users");
                throw;
            }
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            try
            {
                logger.LogInformation(LogMessages.FetchingUser, id);
                var user = await userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    logger.LogWarning(LogMessages.UserNotFound, id);
                    return null;
                }

                return mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch user with ID: {UserId}", id);
                throw;
            }
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            try
            {
                logger.LogInformation(LogMessages.CreatingUser, dto.FullName);

                var user = mapper.Map<User>(dto);
                user.Id = Guid.NewGuid().ToString();

                await userRepository.AddAsync(user);
                await userRepository.SaveAsync();

                logger.LogInformation(LogMessages.UserCreated, user.Id);
                return mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create user: {UserName}", dto.FullName);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(string id, CreateUserDto dto)
        {
            try
            {
                logger.LogInformation(LogMessages.UpdatingUser, id);

                var user = await userRepository.GetByIdAsync(id);
                if (user is null)
                {
                    logger.LogWarning(LogMessages.UserNotFound, id);
                    return false;
                }

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Email = dto.Email;

                userRepository.Update(user);
                await userRepository.SaveAsync();

                logger.LogInformation(LogMessages.UserUpdated, id);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update user with ID: {UserId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                logger.LogInformation(LogMessages.DeletingUser, id);

                var user = await userRepository.GetByIdAsync(id);
                if (user is null)
                {
                    logger.LogWarning(LogMessages.UserNotFound, id);
                    return false;
                }

                userRepository.Delete(user);
                await userRepository.SaveAsync();

                logger.LogInformation(LogMessages.UserDeleted, id);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete user with ID: {UserId}", id);
                throw;
            }
        }
    }
}