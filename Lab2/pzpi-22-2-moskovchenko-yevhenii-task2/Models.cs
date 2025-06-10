using System.ComponentModel.DataAnnotations.Schema;


public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<UserWorkingDay> UserWorkingDays { get; set; } = new List<UserWorkingDay>();
    public ICollection<UserChangeRequest> UserChangeRequests { get; set; } = new List<UserChangeRequest>();

}



public class DayType
{
    public int DayTypeId { get; set; }
    public string DayTypeName { get; set; }
    public ICollection<WorkingDay> WorkingDays { get; set; }
    public ICollection<ChangeRequest> ChangeRequests { get; set; }
}

//public class WorkingDay
//{
//    public int WorkingDayId { get; set; }
//    public TimeSpan StartTime { get; set; }
//    public TimeSpan EndTime { get; set; }
//    public int DayTypeId { get; set; }
//    public DayType DayType { get; set; }
//    public ICollection<UserWorkingDay> UserWorkingDays { get; set; }
//}
public class WorkingDay
{
    public int WorkingDayId { get; set; }
    public DateTime StartTime { get; set; } 
    public DateTime EndTime { get; set; }   
    public int DayTypeId { get; set; }
    public DayType DayType { get; set; }
    public ICollection<UserWorkingDay> UserWorkingDays { get; set; }
}


public class UserWorkingDay
{
    public int UserWorkingDayId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int WorkingDayId { get; set; }
    public WorkingDay WorkingDay { get; set; }
}
public class ChangeRequest
{
    public int ChangeRequestId { get; set; }
    public DateTime RequestDate { get; set; }
    public string Status { get; set; }
    public int DayTypeId { get; set; }
    public DayType DayType { get; set; }
    public ICollection<UserChangeRequest> UserChangeRequests { get; set; }
    public DateTime StartDate { get; set; } // Дата початку
    public DateTime EndDate { get; set; }   // Дата кінця
    public string Description { get; set; } // Короткий опис

    public ChangeRequest()
    {
        Status = "на опрацюванні";
    }
}



public class UserChangeRequest
{
    public int UserChangeRequestId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int ChangeRequestId { get; set; }
    public ChangeRequest ChangeRequest { get; set; }
}



[Table("SensorData")]
public class SensorData
{
    [Column("id")]
    public int Id { get; set; }

    [Column("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.Now;

    [Column("temperature")]
    public decimal Temperature { get; set; }

    [Column("humidity")]
    public decimal Humidity { get; set; }

    [Column("IsTemperatureAdjustmentEnabled")]
    public bool IsTemperatureAdjustmentEnabled { get; set; } 

    [Column("IsHumidityAdjustmentEnabled")]
    public bool IsHumidityAdjustmentEnabled { get; set; } 
}
