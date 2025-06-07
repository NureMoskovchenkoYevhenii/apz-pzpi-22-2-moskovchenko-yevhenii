using Microsoft.EntityFrameworkCore;

public interface IWorkingDayRepository
{
    void Add(WorkingDay workingDay); // Зміна типу параметра на WorkingDay
    IEnumerable<WorkingDay> GetAll();
    WorkingDay GetById(int workingDayId);
    void Update(WorkingDay workingDay);
    void Delete(int workingDayId);
}
public class WorkingDayRepository : IWorkingDayRepository
{
    private readonly ApplicationDbContext _context;

    public WorkingDayRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(WorkingDay workingDay) 
    {
        _context.WorkingDays.Add(workingDay);
        _context.SaveChanges();
    }

    public IEnumerable<WorkingDay> GetAll()
    {
        return _context.WorkingDays.Include(wd => wd.DayType).ToList();
    }

    public WorkingDay GetById(int workingDayId)
    {
        return _context.WorkingDays.Include(wd => wd.DayType).FirstOrDefault(wd => wd.WorkingDayId == workingDayId);
    }

    public void Update(WorkingDay workingDay)
    {
        _context.WorkingDays.Update(workingDay);
        _context.SaveChanges();
    }

    public void Delete(int workingDayId)
    {
        var workingDay = _context.WorkingDays.Find(workingDayId);
        if (workingDay != null)
        {
            _context.WorkingDays.Remove(workingDay);
            _context.SaveChanges();
        }
    }
}
