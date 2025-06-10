public class UserWorkingDayMapper
{
    public UserWorkingDayDto MapToDto(UserWorkingDay userWorkingDay)
    {
        return new UserWorkingDayDto
        {
            UserWorkingDayId = userWorkingDay.UserWorkingDayId,
            UserId = userWorkingDay.UserId,
            WorkingDayId = userWorkingDay.WorkingDayId
            // Інші властивості...
        };
    }

    public UserWorkingDay MapToEntity(UserWorkingDayDto userWorkingDayDto)
    {
        return new UserWorkingDay
        {
            UserWorkingDayId = userWorkingDayDto.UserWorkingDayId,
            UserId = userWorkingDayDto.UserId,
            WorkingDayId = userWorkingDayDto.WorkingDayId
            // Інші властивості...
        };
    }
}

public class UserWorkingDayDto
{
    public int UserWorkingDayId { get; set; }
    public int UserId { get; set; }
    public int WorkingDayId { get; set; }
    // Інші властивості...
}
