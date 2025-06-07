public class WorkingDayMapper
{
    public WorkingDayDto MapToDto(WorkingDay workingDay)
    {
        return new WorkingDayDto
        {
            WorkingDayId = workingDay.WorkingDayId,
            StartTime = workingDay.StartTime, // Залишається DateTime
            EndTime = workingDay.EndTime,     // Залишається DateTime
            DayTypeId = workingDay.DayTypeId
            // Інші властивості...
        };
    }

    public WorkingDay MapToEntity(WorkingDayDto workingDayDto)
    {
        return new WorkingDay
        {
            WorkingDayId = workingDayDto.WorkingDayId,
            StartTime = workingDayDto.StartTime, // Залишається DateTime
            EndTime = workingDayDto.EndTime,     // Залишається DateTime
            DayTypeId = workingDayDto.DayTypeId
            // Інші властивості...
        };
    }
}


public class WorkingDayDto
{
    public int WorkingDayId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int DayTypeId { get; set; }
    public int UserId { get; set; } // Додано UserId
}
