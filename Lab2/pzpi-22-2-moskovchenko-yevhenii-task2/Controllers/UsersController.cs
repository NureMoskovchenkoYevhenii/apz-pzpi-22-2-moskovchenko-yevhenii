// --- UsersController.cs ---
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization; // Обов'язково додайте цей using
using System.Linq; 
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Застосовуємо авторизацію до всього контролера
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly UserMapper _userMapper;
    private readonly IStringLocalizer<SharedResources> _localizer; // Впроваджуємо локалізатор

    public UsersController(UserService userService, UserMapper userMapper, IStringLocalizer<SharedResources> localizer)
    {
        _userService = userService;
        _userMapper = userMapper;
        _localizer = localizer;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        var userDtos = users.Select(u => _userMapper.MapToDto(u));
        return Ok(userDtos);
    }

    [HttpGet("{id}/working-days-report")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserWorkingDaysReport(int id)
    {
        try
        {
            var report = await _userService.GenerateUserWorkingDaysReport(id);
            return Ok(report);
        }
        catch (Exception ex)
        {
            // Повертаємо локалізовану помилку, якщо користувача не знайдено
            if (ex.Message == _localizer["UserNotFound"])
            {
                return NotFound(_localizer["UserNotFound", id].Value);
            }
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            // Повертаємо локалізовану помилку 404
            return NotFound(_localizer["UserNotFound", id].Value);
        }
        var userDto = _userMapper.MapToDto(user);
        return Ok(userDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            // Повертаємо локалізовану помилку 400
            return BadRequest(_localizer["InvalidData"].Value);
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
        if (userDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        
        var userToUpdate = _userMapper.MapToEntity(userDto);
        await _userService.UpdateUserAsync(id, userToUpdate);

        return NoContent(); // 204 No Content - успішне оновлення
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent(); 
    }
}