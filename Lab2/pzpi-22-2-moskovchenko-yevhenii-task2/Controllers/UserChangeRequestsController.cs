using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserChangeRequestsController : ControllerBase
{
    private readonly UserChangeRequestService _userChangeRequestService;
    private readonly UserChangeRequestMapper _userChangeRequestMapper;

    public UserChangeRequestsController(UserChangeRequestService userChangeRequestService, UserChangeRequestMapper userChangeRequestMapper)
    {
        _userChangeRequestService = userChangeRequestService;
        _userChangeRequestMapper = userChangeRequestMapper;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllUserChangeRequests()
    {
        var userChangeRequests = _userChangeRequestService.GetAllUserChangeRequests();
        var userChangeRequestDtos = userChangeRequests.Select(ucr => _userChangeRequestMapper.MapToDto(ucr));
        return Ok(userChangeRequestDtos);
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetUserChangeRequestById(int id)
    {
        var userChangeRequest = _userChangeRequestService.GetUserChangeRequestById(id);
        if (userChangeRequest == null)
        {
            return NotFound();
        }
        var userChangeRequestDto = _userChangeRequestMapper.MapToDto(userChangeRequest);
        return Ok(userChangeRequestDto);
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddUserChangeRequest(UserChangeRequestDto userChangeRequestDto)
    {
        var userChangeRequest = _userChangeRequestMapper.MapToEntity(userChangeRequestDto);
        _userChangeRequestService.AddUserChangeRequest(userChangeRequest);
        return CreatedAtAction(nameof(GetUserChangeRequestById), new { id = userChangeRequest.UserChangeRequestId }, userChangeRequestDto);
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateUserChangeRequest(int id, UserChangeRequestDto updatedUserChangeRequestDto)
    {
        var updatedUserChangeRequest = _userChangeRequestMapper.MapToEntity(updatedUserChangeRequestDto);
        _userChangeRequestService.UpdateUserChangeRequest(id, updatedUserChangeRequest);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteUserChangeRequest(int id)
    {
        _userChangeRequestService.DeleteUserChangeRequest(id);
        return NoContent();
    }
}
