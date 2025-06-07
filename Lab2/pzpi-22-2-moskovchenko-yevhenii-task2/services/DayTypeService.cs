public class DayTypeService
{
    private readonly IDayTypeRepository _dayTypeRepository;

    public DayTypeService(IDayTypeRepository dayTypeRepository)
    {
        _dayTypeRepository = dayTypeRepository;
    }

    public void AddDayType(DayType dayType)
    {
        _dayTypeRepository.Add(dayType);
    }

    public IEnumerable<DayType> GetAllDayTypes()
    {
        return _dayTypeRepository.GetAll();
    }

    public DayType GetDayTypeById(int dayTypeId)
    {
        return _dayTypeRepository.GetById(dayTypeId);
    }

    public void UpdateDayType(int dayTypeId, DayType updatedDayType)
    {
        var dayType = _dayTypeRepository.GetById(dayTypeId);
        if (dayType != null)
        {
            dayType.DayTypeName = updatedDayType.DayTypeName;
            _dayTypeRepository.Update(dayType);
        }
    }

    public void DeleteDayType(int dayTypeId)
    {
        _dayTypeRepository.Delete(dayTypeId);
    }
}
