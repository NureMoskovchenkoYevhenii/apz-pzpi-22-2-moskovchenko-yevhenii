using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DayTypesController : ControllerBase
{
    private readonly DayTypeService _dayTypeService;
    private readonly DayTypeMapper _dayTypeMapper;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public DayTypesController(DayTypeService dayTypeService, DayTypeMapper dayTypeMapper, IStringLocalizer<SharedResources> localizer)
    {
        _dayTypeService = dayTypeService;
        _dayTypeMapper = dayTypeMapper;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDayTypes()
    {
        var dayTypes = await _dayTypeService.GetAllDayTypesAsync();
        var dayTypeDtos = dayTypes.Select(dayType => _dayTypeMapper.MapToDto(dayType));
        return Ok(dayTypeDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDayTypeById(int id)
    {
        var dayType = await _dayTypeService.GetDayTypeByIdAsync(id);
        if (dayType == null)
        {
            return NotFound(_localizer["DayTypeNotFound", id].Value);
        }
        var dayTypeDto = _dayTypeMapper.MapToDto(dayType);
        return Ok(dayTypeDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddDayType([FromBody] DayTypeDto dayTypeDto)
    {
        if (dayTypeDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        var dayType = _dayTypeMapper.MapToEntity(dayTypeDto);
        var createdDayType = await _dayTypeService.AddDayTypeAsync(dayType);
        var createdDto = _dayTypeMapper.MapToDto(createdDayType);
        return CreatedAtAction(nameof(GetDayTypeById), new { id = createdDayType.DayTypeId }, createdDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDayType(int id, [FromBody] DayTypeDto updatedDayTypeDto)
    {
        var updatedDayType = _dayTypeMapper.MapToEntity(updatedDayTypeDto);
        await _dayTypeService.UpdateDayTypeAsync(id, updatedDayType);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDayType(int id)
    {
        await _dayTypeService.DeleteDayTypeAsync(id);
        return NoContent();
    }
}