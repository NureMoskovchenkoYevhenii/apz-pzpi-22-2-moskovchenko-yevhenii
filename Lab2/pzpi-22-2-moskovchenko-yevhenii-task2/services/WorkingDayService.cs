using Microsoft.Extensions.Localization;

public class WorkingDayService
{
    private readonly IWorkingDayRepository _workingDayRepository;
    private readonly IUserWorkingDayRepository _userWorkingDayRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public WorkingDayService(
        IWorkingDayRepository workingDayRepository,
        IUserWorkingDayRepository userWorkingDayRepository,
        IStringLocalizer<SharedResources> localizer)
    {
        _workingDayRepository = workingDayRepository;
        _userWorkingDayRepository = userWorkingDayRepository;
        _localizer = localizer;
    }

    public async Task<WorkingDay> AddWorkingDayAsync(WorkingDay workingDay, int userId)
    {
        var createdWorkingDay = await _workingDayRepository.AddAsync(workingDay);

        var userWorkingDay = new UserWorkingDay
        {
            UserId = userId,
            WorkingDayId = createdWorkingDay.WorkingDayId
        };

        await _userWorkingDayRepository.AddAsync(userWorkingDay);
        return createdWorkingDay;
    }

    public async Task<IEnumerable<WorkingDay>> GetAllWorkingDaysAsync()
    {
        return await _workingDayRepository.GetAllAsync();
    }

    public async Task<WorkingDay> GetWorkingDayByIdAsync(int workingDayId)
    {
        return await _workingDayRepository.GetByIdAsync(workingDayId);
    }

    public async Task UpdateWorkingDayAsync(int workingDayId, WorkingDay updatedWorkingDay)
    {
        var workingDay = await _workingDayRepository.GetByIdAsync(workingDayId);
        if (workingDay != null)
        {
            workingDay.StartTime = updatedWorkingDay.StartTime;
            workingDay.EndTime = updatedWorkingDay.EndTime;
            workingDay.DayTypeId = updatedWorkingDay.DayTypeId;
            await _workingDayRepository.UpdateAsync(workingDay);
        }
    }

    public async Task DeleteWorkingDayAsync(int workingDayId)
    {
        await _workingDayRepository.DeleteAsync(workingDayId);
    }
}