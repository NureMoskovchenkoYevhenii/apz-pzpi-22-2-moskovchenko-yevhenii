using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChangeRequestsController : ControllerBase
{
    private readonly ChangeRequestService _changeRequestService;
    private readonly ChangeRequestMapper _changeRequestMapper;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public ChangeRequestsController(ChangeRequestService changeRequestService, ChangeRequestMapper changeRequestMapper, IStringLocalizer<SharedResources> localizer)
    {
        _changeRequestService = changeRequestService;
        _changeRequestMapper = changeRequestMapper;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllChangeRequests()
    {
        var changeRequests = await _changeRequestService.GetAllChangeRequestsAsync();
        var changeRequestDtos = changeRequests.Select(cr => _changeRequestMapper.MapToDto(cr));
        return Ok(changeRequestDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChangeRequestById(int id)
    {
        var changeRequest = await _changeRequestService.GetChangeRequestByIdAsync(id);
        if (changeRequest == null)
        {
            return NotFound(_localizer["ChangeRequestNotFound", id].Value);
        }
        var changeRequestDto = _changeRequestMapper.MapToDto(changeRequest);
        return Ok(changeRequestDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddChangeRequest([FromBody] ChangeRequestDto changeRequestDto)
    {
        if (changeRequestDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        var changeRequest = _changeRequestMapper.MapToEntity(changeRequestDto);
        await _changeRequestService.AddChangeRequestAsync(changeRequest, changeRequestDto.UserId);
        return CreatedAtAction(nameof(GetChangeRequestById), new { id = changeRequest.ChangeRequestId }, changeRequestDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateChangeRequest(int id, [FromBody] ChangeRequestDto updatedChangeRequestDto)
    {
        if (updatedChangeRequestDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        var updatedChangeRequest = _changeRequestMapper.MapToEntity(updatedChangeRequestDto);
        await _changeRequestService.UpdateChangeRequestAsync(id, updatedChangeRequest);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteChangeRequest(int id)
    {
        await _changeRequestService.DeleteChangeRequestAsync(id);
        return NoContent();
    }
}