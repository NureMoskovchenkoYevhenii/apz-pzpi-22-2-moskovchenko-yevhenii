// --- UserService.cs ---
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Додано для Where
using System; // Додано для DateTime
using Microsoft.Extensions.Localization;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UserService(IUserRepository userRepository, IStringLocalizer<SharedResources> localizer)
    {
        _userRepository = userRepository;
        _localizer = localizer;
    }

    public async Task<User> Authenticate(string username, string password)
    {
        var user = await _userRepository.FindByUsernameAsync(username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }
    
    // Всі методи тепер асинхронні і мають суфікс Async
    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task UpdateUserAsync(int userId, User updatedUser)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Role = updatedUser.Role;

            if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                user.PasswordHash = HashPassword(updatedUser.PasswordHash);
            }

            await _userRepository.UpdateAsync(user);
        }
    }

     public async Task DeleteUserAsync(int userId)
    {
        await _userRepository.DeleteAsync(userId);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Метод для звіту теж може бути асинхронним
    public async Task<string> GenerateUserWorkingDaysReport(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new Exception(_localizer["UserNotFound"]);
        }

        var report = $"Report for {user.FirstName} {user.LastName}\n";

        var lastWeekWorkingDays = user.UserWorkingDays?
            .Where(uwd => uwd.WorkingDay.StartTime >= DateTime.Now.AddDays(-7))
            .ToList() ?? new List<UserWorkingDay>();

        double totalHours = 0;
        foreach (var workingDay in lastWeekWorkingDays)
        {
            var hours = (workingDay.WorkingDay.EndTime - workingDay.WorkingDay.StartTime).TotalHours;
            totalHours += hours;
            report += $"Date: {workingDay.WorkingDay.StartTime.ToShortDateString()}, " +
                      $"Start: {workingDay.WorkingDay.StartTime.ToShortTimeString()}, " +
                      $"End: {workingDay.WorkingDay.EndTime.ToShortTimeString()}, " +
                      $"Hours: {hours}\n";
        }
        report += $"Total Hours in Last 7 Days: {totalHours}\n";

        var userChangeRequests = user.UserChangeRequests ?? new List<UserChangeRequest>();
        report += "Change Requests:\n";
        foreach (var userChangeRequest in userChangeRequests)
        {
            var request = userChangeRequest.ChangeRequest;
            report += $"Request Date: {request.RequestDate.ToShortDateString()}, " +
                      $"Status: {request.Status}, " +
                      $"Description: {request.Description}\n";
        }

        return report;
    }
}