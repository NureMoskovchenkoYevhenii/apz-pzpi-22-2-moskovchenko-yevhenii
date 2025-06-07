using Microsoft.EntityFrameworkCore;

public interface IUserChangeRequestRepository
{
    void Add(UserChangeRequest userChangeRequest);
    IEnumerable<UserChangeRequest> GetAll();
    UserChangeRequest GetById(int userChangeRequestId);
    void Update(UserChangeRequest userChangeRequest);
    void Delete(int userChangeRequestId);
}

public class UserChangeRequestRepository : IUserChangeRequestRepository
{
    private readonly ApplicationDbContext _context;

    public UserChangeRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(UserChangeRequest userChangeRequest)
    {
        _context.UserChangeRequests.Add(userChangeRequest);
        _context.SaveChanges();
    }

    public IEnumerable<UserChangeRequest> GetAll()
    {
        return _context.UserChangeRequests
            .Include(ucr => ucr.User)
            .Include(ucr => ucr.ChangeRequest)
            .ToList();
    }

    public UserChangeRequest GetById(int userChangeRequestId)
    {
        return _context.UserChangeRequests
            .Include(ucr => ucr.User)
            .Include(ucr => ucr.ChangeRequest)
            .FirstOrDefault(ucr => ucr.UserChangeRequestId == userChangeRequestId);
    }

    public void Update(UserChangeRequest userChangeRequest)
    {
        _context.UserChangeRequests.Update(userChangeRequest);
        _context.SaveChanges();
    }

    public void Delete(int userChangeRequestId)
    {
        var userChangeRequest = _context.UserChangeRequests.Find(userChangeRequestId);
        if (userChangeRequest != null)
        {
            _context.UserChangeRequests.Remove(userChangeRequest);
            _context.SaveChanges();
        }
    }
}
