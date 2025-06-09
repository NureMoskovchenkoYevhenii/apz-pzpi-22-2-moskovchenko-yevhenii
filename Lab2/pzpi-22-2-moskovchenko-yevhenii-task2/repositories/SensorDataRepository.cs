using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;

public interface ISensorDataRepository
{
    Task AddAsync(SensorData sensorData);
    Task<IEnumerable<SensorData>> GetAllAsync();
    Task<SensorData> GetByIdAsync(int id);
    Task UpdateAsync(SensorData sensorData);
    Task DeleteAsync(int id);
    Task<SensorData> GetLastSensorDataAsync();
}

public class SensorDataRepository : ISensorDataRepository
{
    private readonly ApplicationDbContext _context;

    public SensorDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(SensorData sensorData)
    {
        await _context.SensorData.AddAsync(sensorData);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SensorData>> GetAllAsync()
    {
        return await _context.SensorData.ToListAsync();
    }

    public async Task<SensorData> GetByIdAsync(int id)
    {
        return await _context.SensorData.FindAsync(id);
    }

    public async Task UpdateAsync(SensorData sensorData)
    {
        _context.SensorData.Update(sensorData);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sensorData = await _context.SensorData.FindAsync(id);
        if (sensorData != null)
        {
            _context.SensorData.Remove(sensorData);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<SensorData> GetLastSensorDataAsync()
    {
        return await _context.SensorData.OrderByDescending(s => s.Timestamp).FirstOrDefaultAsync();
    }
}