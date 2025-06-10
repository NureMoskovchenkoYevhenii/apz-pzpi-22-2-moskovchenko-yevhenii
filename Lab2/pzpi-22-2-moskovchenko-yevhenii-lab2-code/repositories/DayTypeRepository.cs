using Microsoft.EntityFrameworkCore;

public interface IDayTypeRepository
{
    Task<DayType> AddAsync(DayType dayType);
    Task<IEnumerable<DayType>> GetAllAsync();
    Task<DayType> GetByIdAsync(int dayTypeId);
    Task UpdateAsync(DayType dayType);
    Task DeleteAsync(int dayTypeId);
}




public class DayTypeRepository : IDayTypeRepository
{
    private readonly ApplicationDbContext _context;

    public DayTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DayType> AddAsync(DayType dayType)
    {
        await _context.DayTypes.AddAsync(dayType);
        await _context.SaveChangesAsync();
        return dayType;
    }

    public async Task<IEnumerable<DayType>> GetAllAsync()
    {
        return await _context.DayTypes.ToListAsync();
    }

    public async Task<DayType> GetByIdAsync(int dayTypeId)
    {
        return await _context.DayTypes.FindAsync(dayTypeId);
    }

    public async Task UpdateAsync(DayType dayType)
    {
        _context.DayTypes.Update(dayType);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int dayTypeId)
    {
        var dayType = await _context.DayTypes.FindAsync(dayTypeId);
        if (dayType != null)
        {
            _context.DayTypes.Remove(dayType);
            await _context.SaveChangesAsync();
        }
    }
}