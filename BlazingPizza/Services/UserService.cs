using Microsoft.EntityFrameworkCore;

namespace BlazingPizza.Services;

public interface IUserService
{
    Task<List<string>> GetAllActiveUserIdsAsync();
    Task<List<CarrierStoreUser>> GetAllActiveUsersAsync();
    Task<int> GetActiveUserCountAsync();
    Task<CarrierStoreUser?> GetUserByIdAsync(string userId);
}

public class UserService : IUserService
{
    private readonly PizzaStoreContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(PizzaStoreContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<string>> GetAllActiveUserIdsAsync()
    {
        try
        {
            _logger.LogInformation("Getting all active user IDs");
            
            var userIds = await _context.Users
                .Where(u => u.Role != "Admin") // Exclude admin users from broadcasts
                .Select(u => u.Id)
                .ToListAsync();
                
            _logger.LogInformation($"Found {userIds.Count} active users");
            return userIds;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get active user IDs");
            return new List<string>();
        }
    }

    public async Task<List<CarrierStoreUser>> GetAllActiveUsersAsync()
    {
        try
        {
            _logger.LogInformation("Getting all active users");
            
            var users = await _context.Users
                .Where(u => u.Role != "Admin") // Exclude admin users
                .ToListAsync();
                
            _logger.LogInformation($"Found {users.Count} active users");
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get active users");
            return new List<CarrierStoreUser>();
        }
    }

    public async Task<int> GetActiveUserCountAsync()
    {
        try
        {
            var count = await _context.Users
                .Where(u => u.Role != "Admin")
                .CountAsync();
                
            _logger.LogInformation($"Active user count: {count}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get active user count");
            return 0;
        }
    }

    public async Task<CarrierStoreUser?> GetUserByIdAsync(string userId)
    {
        try
        {
            return await _context.Users.FindAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get user by ID: {userId}");
            return null;
        }
    }
} 