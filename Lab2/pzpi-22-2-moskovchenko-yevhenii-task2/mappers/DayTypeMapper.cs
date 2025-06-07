public class DayTypeMapper
{
    public DayTypeDto MapToDto(DayType dayType)
    {
        return new DayTypeDto
        {
            DayTypeId = dayType.DayTypeId,
            DayTypeName = dayType.DayTypeName
            // Інші властивості...
        };
    }

    public DayType MapToEntity(DayTypeDto dayTypeDto)
    {
        return new DayType
        {
            DayTypeId = dayTypeDto.DayTypeId,
            DayTypeName = dayTypeDto.DayTypeName
            // Інші властивості...
        };
    }
}

public class DayTypeDto
{
    public int DayTypeId { get; set; }
    public string DayTypeName { get; set; }
    // Інші властивості...
}
