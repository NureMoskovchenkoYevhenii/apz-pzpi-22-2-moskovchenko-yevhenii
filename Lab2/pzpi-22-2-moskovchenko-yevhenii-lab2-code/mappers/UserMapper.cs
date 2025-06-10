public class UserMapper
{
    public UserDto MapToDto(User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role
            // Не включаємо пароль у DTO для безпеки
        };
    }

    public User MapToEntity(UserDto userDto)
    {
        return new User
        {
            UserId = userDto.UserId,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            PhoneNumber = userDto.PhoneNumber,
            Role = userDto.Role,
            PasswordHash = HashPassword(userDto.Password) // Хешування пароля
        };
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}

public class UserDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public string Password { get; set; } // Додано для пароля
    // Інші властивості...
}

