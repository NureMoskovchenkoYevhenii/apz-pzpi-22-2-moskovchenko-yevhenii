using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserWorkingDaysController : ControllerBase
{
    private readonly UserWorkingDayService _userWorkingDayService;
    private readonly UserWorkingDayMapper _userWorkingDayMapper;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UserWorkingDaysController(UserWorkingDayService userWorkingDayService, UserWorkingDayMapper userWorkingDayMapper, IStringLocalizer<SharedResources> localizer)
    {
        _userWorkingDayService = userWorkingDayService;
        _userWorkingDayMapper = userWorkingDayMapper;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserWorkingDays()
    {
        var userWorkingDays = await _userWorkingDayService.GetAllUserWorkingDaysAsync();
        var userWorkingDayDtos = userWorkingDays.Select(uwd => _userWorkingDayMapper.MapToDto(uwd));
        return Ok(userWorkingDayDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserWorkingDayById(int id)
    {
        var userWorkingDay = await _userWorkingDayService.GetUserWorkingDayByIdAsync(id);
        if (userWorkingDay == null)
        {
            return NotFound(_localizer["UserWorkingDayNotFound", id].Value);
        }
        var userWorkingDayDto = _userWorkingDayMapper.MapToDto(userWorkingDay);
        return Ok(userWorkingDayDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddUserWorkingDay([FromBody] UserWorkingDayDto userWorkingDayDto)
    {
        if (userWorkingDayDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        var userWorkingDay = _userWorkingDayMapper.MapToEntity(userWorkingDayDto);
        var createdUserWorkingDay = await _userWorkingDayService.AddUserWorkingDayAsync(userWorkingDay);
        var createdDto = _userWorkingDayMapper.MapToDto(createdUserWorkingDay);
        return CreatedAtAction(nameof(GetUserWorkingDayById), new { id = createdUserWorkingDay.UserWorkingDayId }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserWorkingDay(int id, [FromBody] UserWorkingDayDto updatedUserWorkingDayDto)
    {
        var updatedUserWorkingDay = _userWorkingDayMapper.MapToEntity(updatedUserWorkingDayDto);
        await _userWorkingDayService.UpdateUserWorkingDayAsync(id, updatedUserWorkingDay);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUserWorkingDay(int id)
    {
        await _userWorkingDayService.DeleteUserWorkingDayAsync(id);
        return NoContent();
    }
}