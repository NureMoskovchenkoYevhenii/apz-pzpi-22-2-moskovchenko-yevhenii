// --- UsersController.cs ---
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq; 
using System.Threading.Tasks;

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
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        var userDtos = users.Select(u => _userMapper.MapToDto(u));
        return Ok(userDtos);
    }

    [HttpGet("{id}/working-days-report")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserWorkingDaysReport(int id) // Змінено на async Task
    {
        try
        {
            var report = await _userService.GenerateUserWorkingDaysReport(id); // Додано await
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var userDto = _userMapper.MapToDto(user);
        return Ok(userDto);
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("User data is null.");
        }

        var user = _userMapper.MapToEntity(userDto);
        await _userService.AddUserAsync(user);
        var createdUserDto = _userMapper.MapToDto(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, createdUserDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
    {
        var userToUpdate = _userMapper.MapToEntity(userDto);
        await _userService.UpdateUserAsync(id, userToUpdate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}