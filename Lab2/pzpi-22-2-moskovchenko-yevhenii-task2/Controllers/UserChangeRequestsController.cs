using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserChangeRequestsController : ControllerBase
{
    private readonly UserChangeRequestService _userChangeRequestService;
    private readonly UserChangeRequestMapper _userChangeRequestMapper;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UserChangeRequestsController(UserChangeRequestService userChangeRequestService, UserChangeRequestMapper userChangeRequestMapper, IStringLocalizer<SharedResources> localizer)
    {
        _userChangeRequestService = userChangeRequestService;
        _userChangeRequestMapper = userChangeRequestMapper;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserChangeRequests()
    {
        var userChangeRequests = await _userChangeRequestService.GetAllUserChangeRequestsAsync();
        var userChangeRequestDtos = userChangeRequests.Select(ucr => _userChangeRequestMapper.MapToDto(ucr));
        return Ok(userChangeRequestDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserChangeRequestById(int id)
    {
        var userChangeRequest = await _userChangeRequestService.GetUserChangeRequestByIdAsync(id);
        if (userChangeRequest == null)
        {
            return NotFound(_localizer["UserChangeRequestNotFound", id].Value);
        }
        var userChangeRequestDto = _userChangeRequestMapper.MapToDto(userChangeRequest);
        return Ok(userChangeRequestDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddUserChangeRequest([FromBody] UserChangeRequestDto userChangeRequestDto)
    {
        if (userChangeRequestDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        var userChangeRequest = _userChangeRequestMapper.MapToEntity(userChangeRequestDto);
        var createdRequest = await _userChangeRequestService.AddUserChangeRequestAsync(userChangeRequest);
        var createdDto = _userChangeRequestMapper.MapToDto(createdRequest);
        return CreatedAtAction(nameof(GetUserChangeRequestById), new { id = createdRequest.UserChangeRequestId }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserChangeRequest(int id, [FromBody] UserChangeRequestDto updatedUserChangeRequestDto)
    {
        var updatedUserChangeRequest = _userChangeRequestMapper.MapToEntity(updatedUserChangeRequestDto);
        await _userChangeRequestService.UpdateUserChangeRequestAsync(id, updatedUserChangeRequest);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserChangeRequest(int id)
    {
        await _userChangeRequestService.DeleteUserChangeRequestAsync(id);
        return NoContent();
    }
}