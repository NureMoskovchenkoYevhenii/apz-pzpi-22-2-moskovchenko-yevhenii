using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IChangeRequestRepository
{
    Task<ChangeRequest> AddAsync(ChangeRequest changeRequest);
    Task<IEnumerable<ChangeRequest>> GetAllAsync();
    Task<ChangeRequest> GetByIdAsync(int changeRequestId);
    Task UpdateAsync(ChangeRequest changeRequest);
    Task DeleteAsync(int changeRequestId);
}



public class ChangeRequestRepository : IChangeRequestRepository
{
    private readonly ApplicationDbContext _context;

    public ChangeRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ChangeRequest> AddAsync(ChangeRequest changeRequest)
    {
        await _context.ChangeRequests.AddAsync(changeRequest);
        await _context.SaveChangesAsync();
        return changeRequest;
    }

    public async Task<IEnumerable<ChangeRequest>> GetAllAsync()
    {
        return await _context.ChangeRequests.Include(cr => cr.DayType).ToListAsync();
    }

    public async Task<ChangeRequest> GetByIdAsync(int changeRequestId)
    {
        return await _context.ChangeRequests
            .Include(cr => cr.UserChangeRequests)
            .ThenInclude(ucr => ucr.User)
            .FirstOrDefaultAsync(cr => cr.ChangeRequestId == changeRequestId);
    }

    public async Task UpdateAsync(ChangeRequest changeRequest)
    {
        _context.ChangeRequests.Update(changeRequest);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int changeRequestId)
    {
        var changeRequest = await _context.ChangeRequests.FindAsync(changeRequestId);
        if (changeRequest != null)
        {
            _context.ChangeRequests.Remove(changeRequest);
            await _context.SaveChangesAsync();
        }
    }
}