using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IChangeRequestRepository _changeRequestRepository;

    public UserService(IUserRepository userRepository, IChangeRequestRepository changeRequestRepository)
    {
        _userRepository = userRepository;
        _changeRequestRepository = changeRequestRepository;
    }

    public async Task<User> Authenticate(string username, string password)
    {
        // 1. Отримуємо користувача з репозиторію
        var user = await _userRepository.FindByUsernameAsync(username);

        // 2. Виконуємо бізнес-логіку перевірки пароля
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null; // Автентифікація не пройшла
        }

        // Автентифікація успішна, повертаємо об'єкт користувача
        return user;
    }
    
    public void AddUser(User user)
    {
        user.PasswordHash = HashPassword(user.PasswordHash);
        _userRepository.Add(user);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.GetAll();
    }

    public User GetUserById(int userId)
    {
        return _userRepository.GetById(userId);
    }

    public void UpdateUser(int userId, User updatedUser)
    {
        var user = _userRepository.GetById(userId);
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

            _userRepository.Update(user);
        }
    }

    public void DeleteUser(int userId)
    {
        _userRepository.Delete(userId);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public string GenerateUserWorkingDaysReport(int userId)
    {
        var user = _userRepository.GetById(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var report = $"Report for {user.FirstName} {user.LastName}\n";

        // Ensure UserWorkingDays is not null
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

        // Ensure UserChangeRequests is not null
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
