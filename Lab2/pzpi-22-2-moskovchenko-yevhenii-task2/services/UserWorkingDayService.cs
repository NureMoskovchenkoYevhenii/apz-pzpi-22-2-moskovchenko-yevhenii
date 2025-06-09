using Microsoft.Extensions.Localization;

public class UserWorkingDayService
{
    private readonly IUserWorkingDayRepository _userWorkingDayRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UserWorkingDayService(IUserWorkingDayRepository userWorkingDayRepository, IStringLocalizer<SharedResources> localizer)
    {
        _userWorkingDayRepository = userWorkingDayRepository;
        _localizer = localizer;
    }

    public async Task<UserWorkingDay> AddUserWorkingDayAsync(UserWorkingDay userWorkingDay)
    {
        return await _userWorkingDayRepository.AddAsync(userWorkingDay);
    }

    public async Task<IEnumerable<UserWorkingDay>> GetAllUserWorkingDaysAsync()
    {
        return await _userWorkingDayRepository.GetAllAsync();
    }

    public async Task<UserWorkingDay> GetUserWorkingDayByIdAsync(int userWorkingDayId)
    {
        return await _userWorkingDayRepository.GetByIdAsync(userWorkingDayId);
    }

    public async Task UpdateUserWorkingDayAsync(int userWorkingDayId, UserWorkingDay updatedUserWorkingDay)
    {
        var userWorkingDay = await _userWorkingDayRepository.GetByIdAsync(userWorkingDayId);
        if (userWorkingDay != null)
        {
            userWorkingDay.UserId = updatedUserWorkingDay.UserId;
            userWorkingDay.WorkingDayId = updatedUserWorkingDay.WorkingDayId;
            await _userWorkingDayRepository.UpdateAsync(userWorkingDay);
        }
    }

    public async Task DeleteUserWorkingDayAsync(int userWorkingDayId)
    {
        await _userWorkingDayRepository.DeleteAsync(userWorkingDayId);
    }
}