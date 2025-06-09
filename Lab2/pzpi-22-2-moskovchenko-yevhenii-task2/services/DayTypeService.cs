using Microsoft.Extensions.Localization;

public class DayTypeService
{
    private readonly IDayTypeRepository _dayTypeRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public DayTypeService(IDayTypeRepository dayTypeRepository, IStringLocalizer<SharedResources> localizer)
    {
        _dayTypeRepository = dayTypeRepository;
        _localizer = localizer;
    }

    public async Task<DayType> AddDayTypeAsync(DayType dayType)
    {
        return await _dayTypeRepository.AddAsync(dayType);
    }

    public async Task<IEnumerable<DayType>> GetAllDayTypesAsync()
    {
        return await _dayTypeRepository.GetAllAsync();
    }

    public async Task<DayType> GetDayTypeByIdAsync(int dayTypeId)
    {
        return await _dayTypeRepository.GetByIdAsync(dayTypeId);
    }

    public async Task UpdateDayTypeAsync(int dayTypeId, DayType updatedDayType)
    {
        var dayType = await _dayTypeRepository.GetByIdAsync(dayTypeId);
        if (dayType != null)
        {
            dayType.DayTypeName = updatedDayType.DayTypeName;
            await _dayTypeRepository.UpdateAsync(dayType);
        }
    }

    public async Task DeleteDayTypeAsync(int dayTypeId)
    {
        await _dayTypeRepository.DeleteAsync(dayTypeId);
    }
}