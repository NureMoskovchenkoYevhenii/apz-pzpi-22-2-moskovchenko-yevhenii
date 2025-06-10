using Microsoft.EntityFrameworkCore;

public interface IUserChangeRequestRepository
{
    Task<UserChangeRequest> AddAsync(UserChangeRequest userChangeRequest);
    Task<IEnumerable<UserChangeRequest>> GetAllAsync();
    Task<UserChangeRequest> GetByIdAsync(int userChangeRequestId);
    Task UpdateAsync(UserChangeRequest userChangeRequest);
    Task DeleteAsync(int userChangeRequestId);
}



public class UserChangeRequestRepository : IUserChangeRequestRepository
{
    private readonly ApplicationDbContext _context;

    public UserChangeRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserChangeRequest> AddAsync(UserChangeRequest userChangeRequest)
    {
        await _context.UserChangeRequests.AddAsync(userChangeRequest);
        await _context.SaveChangesAsync();
        return userChangeRequest;
    }

    public async Task<IEnumerable<UserChangeRequest>> GetAllAsync()
    {
        return await _context.UserChangeRequests
            .Include(ucr => ucr.User)
            .Include(ucr => ucr.ChangeRequest)
            .ToListAsync();
    }

    public async Task<UserChangeRequest> GetByIdAsync(int userChangeRequestId)
    {
        return await _context.UserChangeRequests
            .Include(ucr => ucr.User)
            .Include(ucr => ucr.ChangeRequest)
            .FirstOrDefaultAsync(ucr => ucr.UserChangeRequestId == userChangeRequestId);
    }

    public async Task UpdateAsync(UserChangeRequest userChangeRequest)
    {
        _context.UserChangeRequests.Update(userChangeRequest);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userChangeRequestId)
    {
        var userChangeRequest = await _context.UserChangeRequests.FindAsync(userChangeRequestId);
        if (userChangeRequest != null)
        {
            _context.UserChangeRequests.Remove(userChangeRequest);
            await _context.SaveChangesAsync();
        }
    }
}