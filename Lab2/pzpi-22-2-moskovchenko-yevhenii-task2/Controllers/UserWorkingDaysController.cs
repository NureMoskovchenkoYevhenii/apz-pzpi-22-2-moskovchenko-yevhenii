using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserWorkingDaysController : ControllerBase
{
    private readonly UserWorkingDayService _userWorkingDayService;
    private readonly UserWorkingDayMapper _userWorkingDayMapper;

    public UserWorkingDaysController(UserWorkingDayService userWorkingDayService, UserWorkingDayMapper userWorkingDayMapper)
    {
        _userWorkingDayService = userWorkingDayService;
        _userWorkingDayMapper = userWorkingDayMapper;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllUserWorkingDays()
    {
        var userWorkingDays = _userWorkingDayService.GetAllUserWorkingDays();
        var userWorkingDayDtos = userWorkingDays.Select(uwd => _userWorkingDayMapper.MapToDto(uwd));
        return Ok(userWorkingDayDtos);
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetUserWorkingDayById(int id)
    {
        var userWorkingDay = _userWorkingDayService.GetUserWorkingDayById(id);
        if (userWorkingDay == null)
        {
            return NotFound();
        }
        var userWorkingDayDto = _userWorkingDayMapper.MapToDto(userWorkingDay);
        return Ok(userWorkingDayDto);
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddUserWorkingDay(UserWorkingDayDto userWorkingDayDto)
    {
        var userWorkingDay = _userWorkingDayMapper.MapToEntity(userWorkingDayDto);
        _userWorkingDayService.AddUserWorkingDay(userWorkingDay);
        return CreatedAtAction(nameof(GetUserWorkingDayById), new { id = userWorkingDay.UserWorkingDayId }, userWorkingDayDto);
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateUserWorkingDay(int id, UserWorkingDayDto updatedUserWorkingDayDto)
    {
        var updatedUserWorkingDay = _userWorkingDayMapper.MapToEntity(updatedUserWorkingDayDto);
        _userWorkingDayService.UpdateUserWorkingDay(id, updatedUserWorkingDay);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteUserWorkingDay(int id)
    {
        _userWorkingDayService.DeleteUserWorkingDay(id);
        return NoContent();
    }
}
