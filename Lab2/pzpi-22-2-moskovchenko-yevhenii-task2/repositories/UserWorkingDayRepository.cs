using Microsoft.EntityFrameworkCore;

public interface IUserWorkingDayRepository
{
    void Add(UserWorkingDay userWorkingDay);
    IEnumerable<UserWorkingDay> GetAll();
    UserWorkingDay GetById(int userWorkingDayId);
    void Update(UserWorkingDay userWorkingDay);
    void Delete(int userWorkingDayId);
}

public class UserWorkingDayRepository : IUserWorkingDayRepository
{
    private readonly ApplicationDbContext _context;

    public UserWorkingDayRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(UserWorkingDay userWorkingDay)
    {
        _context.UserWorkingDays.Add(userWorkingDay);
        _context.SaveChanges();
    }

    public IEnumerable<UserWorkingDay> GetAll()
    {
        return _context.UserWorkingDays
            .Include(uwd => uwd.User)
            .Include(uwd => uwd.WorkingDay)
            .ToList();
    }

    public UserWorkingDay GetById(int userWorkingDayId)
    {
        return _context.UserWorkingDays
            .Include(uwd => uwd.User)
            .Include(uwd => uwd.WorkingDay)
            .FirstOrDefault(uwd => uwd.UserWorkingDayId == userWorkingDayId);
    }

    public void Update(UserWorkingDay userWorkingDay)
    {
        _context.UserWorkingDays.Update(userWorkingDay);
        _context.SaveChanges();
    }

    public void Delete(int userWorkingDayId)
    {
        var userWorkingDay = _context.UserWorkingDays.Find(userWorkingDayId);
        if (userWorkingDay != null)
        {
            _context.UserWorkingDays.Remove(userWorkingDay);
            _context.SaveChanges();
        }
    }
}
