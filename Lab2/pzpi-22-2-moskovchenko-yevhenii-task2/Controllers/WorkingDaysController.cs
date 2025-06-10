using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkingDaysController : ControllerBase
{
    private readonly WorkingDayService _workingDayService;
    private readonly WorkingDayMapper _workingDayMapper;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public WorkingDaysController(
        WorkingDayService workingDayService,
        WorkingDayMapper workingDayMapper,
        IStringLocalizer<SharedResources> localizer)
    {
        _workingDayService = workingDayService;
        _workingDayMapper = workingDayMapper;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWorkingDays()
    {
        var workingDays = await _workingDayService.GetAllWorkingDaysAsync();
        var workingDayDtos = workingDays.Select(wd => _workingDayMapper.MapToDto(wd));
        return Ok(workingDayDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkingDayById(int id)
    {
        var workingDay = await _workingDayService.GetWorkingDayByIdAsync(id);
        if (workingDay == null)
        {
            return NotFound(_localizer["WorkingDayNotFound", id].Value);
        }
        var workingDayDto = _workingDayMapper.MapToDto(workingDay);
        return Ok(workingDayDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddWorkingDay([FromBody] WorkingDayDto workingDayDto)
    {
        if (workingDayDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        var workingDay = _workingDayMapper.MapToEntity(workingDayDto);
        var createdWorkingDay = await _workingDayService.AddWorkingDayAsync(workingDay, workingDayDto.UserId);
        var createdDto = _workingDayMapper.MapToDto(createdWorkingDay);
        return CreatedAtAction(nameof(GetWorkingDayById), new { id = createdWorkingDay.WorkingDayId }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkingDay(int id, [FromBody] WorkingDayDto updatedWorkingDayDto)
    {
        if (updatedWorkingDayDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        var updatedWorkingDay = _workingDayMapper.MapToEntity(updatedWorkingDayDto);
        await _workingDayService.UpdateWorkingDayAsync(id, updatedWorkingDay);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteWorkingDay(int id)
    {
        await _workingDayService.DeleteWorkingDayAsync(id);
        return NoContent();
    }
}