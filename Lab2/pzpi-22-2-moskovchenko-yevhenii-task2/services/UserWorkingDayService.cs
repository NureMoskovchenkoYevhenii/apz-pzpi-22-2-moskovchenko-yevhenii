public class UserWorkingDayService
{
    private readonly IUserWorkingDayRepository _userWorkingDayRepository;

    public UserWorkingDayService(IUserWorkingDayRepository userWorkingDayRepository)
    {
        _userWorkingDayRepository = userWorkingDayRepository;
    }

    public void AddUserWorkingDay(UserWorkingDay userWorkingDay)
    {
        _userWorkingDayRepository.Add(userWorkingDay);
    }

    public IEnumerable<UserWorkingDay> GetAllUserWorkingDays()
    {
        return _userWorkingDayRepository.GetAll();
    }

    public UserWorkingDay GetUserWorkingDayById(int userWorkingDayId)
    {
        return _userWorkingDayRepository.GetById(userWorkingDayId);
    }

    public void UpdateUserWorkingDay(int userWorkingDayId, UserWorkingDay updatedUserWorkingDay)
    {
        var userWorkingDay = _userWorkingDayRepository.GetById(userWorkingDayId);
        if (userWorkingDay != null)
        {
            userWorkingDay.UserId = updatedUserWorkingDay.UserId;
            userWorkingDay.WorkingDayId = updatedUserWorkingDay.WorkingDayId;
            _userWorkingDayRepository.Update(userWorkingDay);
        }
    }

    public void DeleteUserWorkingDay(int userWorkingDayId)
    {
        _userWorkingDayRepository.Delete(userWorkingDayId);
    }
}
