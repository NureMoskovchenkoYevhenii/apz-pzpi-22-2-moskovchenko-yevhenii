using Microsoft.EntityFrameworkCore;

public interface IWorkingDayRepository
{
    Task<WorkingDay> AddAsync(WorkingDay workingDay);
    Task<IEnumerable<WorkingDay>> GetAllAsync();
    Task<WorkingDay> GetByIdAsync(int workingDayId);
    Task UpdateAsync(WorkingDay workingDay);
    Task DeleteAsync(int workingDayId);
}


public class WorkingDayRepository : IWorkingDayRepository
{
    private readonly ApplicationDbContext _context;

    public WorkingDayRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkingDay> AddAsync(WorkingDay workingDay)
    {
        await _context.WorkingDays.AddAsync(workingDay);
        await _context.SaveChangesAsync();
        return workingDay;
    }

    public async Task<IEnumerable<WorkingDay>> GetAllAsync()
    {
        return await _context.WorkingDays.Include(wd => wd.DayType).ToListAsync();
    }

    public async Task<WorkingDay> GetByIdAsync(int workingDayId)
    {
        return await _context.WorkingDays.Include(wd => wd.DayType).FirstOrDefaultAsync(wd => wd.WorkingDayId == workingDayId);
    }

    public async Task UpdateAsync(WorkingDay workingDay)
    {
        _context.WorkingDays.Update(workingDay);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int workingDayId)
    {
        var workingDay = await _context.WorkingDays.FindAsync(workingDayId);
        if (workingDay != null)
        {
            _context.WorkingDays.Remove(workingDay);
            await _context.SaveChangesAsync();
        }
    }
}