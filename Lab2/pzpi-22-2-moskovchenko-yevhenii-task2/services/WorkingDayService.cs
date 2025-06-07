public class WorkingDayService
{
    private readonly IWorkingDayRepository _workingDayRepository;
    private readonly IUserWorkingDayRepository _userWorkingDayRepository;

    public WorkingDayService(IWorkingDayRepository workingDayRepository, IUserWorkingDayRepository userWorkingDayRepository)
    {
        _workingDayRepository = workingDayRepository;
        _userWorkingDayRepository = userWorkingDayRepository;
    }

    public void AddWorkingDay(WorkingDay workingDay, int userId)
    {
        _workingDayRepository.Add(workingDay);

        var userWorkingDay = new UserWorkingDay
        {
            UserId = userId,
            WorkingDayId = workingDay.WorkingDayId
        };

        _userWorkingDayRepository.Add(userWorkingDay);
    }



    public IEnumerable<WorkingDay> GetAllWorkingDays()
    {
        return _workingDayRepository.GetAll();
    }

    public WorkingDay GetWorkingDayById(int workingDayId)
    {
        return _workingDayRepository.GetById(workingDayId);
    }

    public void UpdateWorkingDay(int workingDayId, WorkingDay updatedWorkingDay)
    {
        var workingDay = _workingDayRepository.GetById(workingDayId);
        if (workingDay != null)
        {
            workingDay.StartTime = updatedWorkingDay.StartTime;
            workingDay.EndTime = updatedWorkingDay.EndTime;
            workingDay.DayTypeId = updatedWorkingDay.DayTypeId;
            _workingDayRepository.Update(workingDay);
        }
    }

    public void DeleteWorkingDay(int workingDayId)
    {
        _workingDayRepository.Delete(workingDayId);
    }
}
