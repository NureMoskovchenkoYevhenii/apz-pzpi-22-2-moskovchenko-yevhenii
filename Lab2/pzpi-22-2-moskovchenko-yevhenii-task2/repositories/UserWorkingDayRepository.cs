using Microsoft.EntityFrameworkCore;

public interface IUserWorkingDayRepository
{
    Task<UserWorkingDay> AddAsync(UserWorkingDay userWorkingDay);
    Task<IEnumerable<UserWorkingDay>> GetAllAsync();
    Task<UserWorkingDay> GetByIdAsync(int userWorkingDayId);
    Task UpdateAsync(UserWorkingDay userWorkingDay);
    Task DeleteAsync(int userWorkingDayId);
}


public class UserWorkingDayRepository : IUserWorkingDayRepository
{
    private readonly ApplicationDbContext _context;

    public UserWorkingDayRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserWorkingDay> AddAsync(UserWorkingDay userWorkingDay)
    {
        await _context.UserWorkingDays.AddAsync(userWorkingDay);
        await _context.SaveChangesAsync();
        return userWorkingDay;
    }

    public async Task<IEnumerable<UserWorkingDay>> GetAllAsync()
    {
        return await _context.UserWorkingDays
            .Include(uwd => uwd.User)
            .Include(uwd => uwd.WorkingDay)
            .ToListAsync();
    }

    public async Task<UserWorkingDay> GetByIdAsync(int userWorkingDayId)
    {
        return await _context.UserWorkingDays
            .Include(uwd => uwd.User)
            .Include(uwd => uwd.WorkingDay)
            .FirstOrDefaultAsync(uwd => uwd.UserWorkingDayId == userWorkingDayId);
    }

    public async Task UpdateAsync(UserWorkingDay userWorkingDay)
    {
        _context.UserWorkingDays.Update(userWorkingDay);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userWorkingDayId)
    {
        var userWorkingDay = await _context.UserWorkingDays.FindAsync(userWorkingDayId);
        if (userWorkingDay != null)
        {
            _context.UserWorkingDays.Remove(userWorkingDay);
            await _context.SaveChangesAsync();
        }
    }
}