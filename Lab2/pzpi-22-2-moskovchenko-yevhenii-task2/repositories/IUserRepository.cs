using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
     Task<User> FindByUsernameAsync(string username);

    // Асинхронні CRUD-операції
    Task<User> AddAsync(User user);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(int userId);
    Task UpdateAsync(User user);
    Task DeleteAsync(int userId);
}

// --- UserRepository.cs ---

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user; // Повертаємо користувача з оновленим Id
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.UserWorkingDays)
                .ThenInclude(uwd => uwd.WorkingDay)
            .Include(u => u.UserChangeRequests)
                .ThenInclude(ucr => ucr.ChangeRequest)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User> FindByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == username);
    }
}