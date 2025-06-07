using Microsoft.EntityFrameworkCore;

public interface IChangeRequestRepository
{
    void Add(UserChangeRequest userChangeRequest);
    void Add(ChangeRequest changeRequest);
    IEnumerable<ChangeRequest> GetAll();
    ChangeRequest GetById(int changeRequestId);
    void Update(ChangeRequest changeRequest);
    void Delete(int changeRequestId);
}

public class ChangeRequestRepository : IChangeRequestRepository
{
    private readonly ApplicationDbContext _context;

    public ChangeRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(UserChangeRequest userChangeRequest)
    {
        _context.UserChangeRequests.Add(userChangeRequest);
        _context.SaveChanges();
    }
    public void Add(ChangeRequest changeRequest)
    {
        _context.ChangeRequests.Add(changeRequest);
        _context.SaveChanges();
    }


    public IEnumerable<ChangeRequest> GetAll()
    {
        return _context.ChangeRequests.Include(cr => cr.DayType).ToList();
    }

    public ChangeRequest GetById(int changeRequestId)
    {
        return _context.ChangeRequests
     .Include(cr => cr.UserChangeRequests)
     .ThenInclude(ucr => ucr.User)
     .FirstOrDefault(cr => cr.ChangeRequestId == changeRequestId);
    }

    public void Update(ChangeRequest changeRequest)
    {
        _context.ChangeRequests.Update(changeRequest);
        _context.SaveChanges();
    }

    public void Delete(int changeRequestId)
    {
        var changeRequest = _context.ChangeRequests.Find(changeRequestId);
        if (changeRequest != null)
        {
            _context.ChangeRequests.Remove(changeRequest);
            _context.SaveChanges();
        }
    }
}
