using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class WorkingDaysController : ControllerBase
{
    private readonly WorkingDayService _workingDayService;
    private readonly WorkingDayMapper _workingDayMapper;

    public WorkingDaysController(WorkingDayService workingDayService, WorkingDayMapper workingDayMapper)
    {
        _workingDayService = workingDayService;
        _workingDayMapper = workingDayMapper;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllWorkingDays()
    {
        var workingDays = _workingDayService.GetAllWorkingDays();
        var workingDayDtos = workingDays.Select(wd => _workingDayMapper.MapToDto(wd));
        return Ok(workingDayDtos);
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetWorkingDayById(int id)
    {
        var workingDay = _workingDayService.GetWorkingDayById(id);
        if (workingDay == null)
        {
            return NotFound();
        }
        var workingDayDto = _workingDayMapper.MapToDto(workingDay);
        return Ok(workingDayDto);
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddWorkingDay(WorkingDayDto workingDayDto)
    {
        var workingDay = _workingDayMapper.MapToEntity(workingDayDto);
        _workingDayService.AddWorkingDay(workingDay, workingDayDto.UserId);
        return CreatedAtAction(nameof(GetWorkingDayById), new { id = workingDay.WorkingDayId }, workingDayDto);
    }


    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateWorkingDay(int id, WorkingDayDto updatedWorkingDayDto)
    {
        var updatedWorkingDay = _workingDayMapper.MapToEntity(updatedWorkingDayDto);
        _workingDayService.UpdateWorkingDay(id, updatedWorkingDay);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteWorkingDay(int id)
    {
        _workingDayService.DeleteWorkingDay(id);
        return NoContent();
    }
}
