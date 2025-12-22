using Microsoft.AspNetCore.Mvc;
using UserApi.Dtos;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _iUserService;
    public UserController(IUserService iUserService)
    {
        _iUserService = iUserService;
    }
    // GET api/users
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken ct)
    {
        var response = await _iUserService.GetAllAsync(ct);
        return Ok(response);
    }

    // GET api/users/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(Guid id, CancellationToken ct)
    {
        var user = await _iUserService.GetByIdAsync(id, ct);
        return user is not null ? Ok(user) : NotFound();
    }

    // POST api/users
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request, CancellationToken ct)
    {
        try
        {
            var newUser = await _iUserService.CreateAsync(request, ct);
            return Ok(newUser);
        } catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}