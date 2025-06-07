public interface IDayTypeRepository
{
    void Add(DayType dayType);
    IEnumerable<DayType> GetAll();
    DayType GetById(int dayTypeId);
    void Update(DayType dayType);
    void Delete(int dayTypeId);
}

public class DayTypeRepository : IDayTypeRepository
{
    private readonly ApplicationDbContext _context;

    public DayTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(DayType dayType)
    {
        _context.DayTypes.Add(dayType);
        _context.SaveChanges();
    }

    public IEnumerable<DayType> GetAll()
    {
        return _context.DayTypes.ToList();
    }

    public DayType GetById(int dayTypeId)
    {
        return _context.DayTypes.Find(dayTypeId);
    }

    public void Update(DayType dayType)
    {
        _context.DayTypes.Update(dayType);
        _context.SaveChanges();
    }

    public void Delete(int dayTypeId)
    {
        var dayType = _context.DayTypes.Find(dayTypeId);
        if (dayType != null)
        {
            _context.DayTypes.Remove(dayType);
            _context.SaveChanges();
        }
    }
}
