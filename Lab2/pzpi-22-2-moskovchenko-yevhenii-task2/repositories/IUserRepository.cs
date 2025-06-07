using Microsoft.EntityFrameworkCore;

public interface IUserRepository
{
    Task<User> FindByUsernameAsync(string username); // Замість Authenticate
    void Add(User user);
    IEnumerable<User> GetAll();
    User GetById(int userId);
    void Update(User user);
    void Delete(int userId);
}

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public User GetById(int userId)
    {
        return _context.Users
            .Include(u => u.UserWorkingDays)
                .ThenInclude(uwd => uwd.WorkingDay)
            .Include(u => u.UserChangeRequests)
                .ThenInclude(ucr => ucr.ChangeRequest)
            .FirstOrDefault(u => u.UserId == userId);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }

    public async Task<User> FindByUsernameAsync(string username)
    {
        // Шукаємо користувача за його номером телефону
        return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == username);
    }
}
