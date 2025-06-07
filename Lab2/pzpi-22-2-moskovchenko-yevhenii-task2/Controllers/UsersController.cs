using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly UserMapper _userMapper;

    public UsersController(UserService userService, UserMapper userMapper)
    {
        _userService = userService;
        _userMapper = userMapper;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        var userDtos = users.Select(user => _userMapper.MapToDto(user));
        return Ok(userDtos);
    }

    [HttpGet("{id}/working-days-report")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUserWorkingDaysReport(int id)
    {
        try
        {
            var report = _userService.GenerateUserWorkingDaysReport(id);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        var userDto = _userMapper.MapToDto(user);
        return Ok(userDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AddUser(UserDto userDto)
    {
        var user = _userMapper.MapToEntity(userDto);
        _userService.AddUser(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, userDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateUser(int id, UserDto updatedUserDto)
    {
        var updatedUser = _userMapper.MapToEntity(updatedUserDto);
        _userService.UpdateUser(id, updatedUser);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteUser(int id)
    {
        _userService.DeleteUser(id);
        return NoContent();
    }
}
