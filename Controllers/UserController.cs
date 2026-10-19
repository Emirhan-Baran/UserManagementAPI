using Microsoft.AspNetCore.Mvc;
using UseManagementAPI.Models;

namespace UseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private static readonly List<User> Users =
    [
        new User
        {
            Id = 1,
            FullName = "John Doe",
            Email = "john@example.com"
        },
        new User
        {
            Id = 2,
            FullName = "Jane Smith",
            Email = "jane@example.com"
        }
    ];

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = Users.FirstOrDefault(x => x.Id == id);

        if (user == null)
            return NotFound("User not found.");

        return Ok(user);
    }

    [HttpPost]
    public IActionResult Create(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (Users.Any(x =>
            x.Email.ToLower() == user.Email.ToLower()))
        {
            return BadRequest("Email already exists.");
        }

        user.Id = Users.Max(x => x.Id) + 1;

        Users.Add(user);

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(
        int id,
        User updatedUser)
    {
        var user = Users.FirstOrDefault(
            x => x.Id == id);

        if (user == null)
            return NotFound("User not found.");

        user.FullName = updatedUser.FullName;
        user.Email = updatedUser.Email;

        return Ok("User updated successfully.");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = Users.FirstOrDefault(
            x => x.Id == id);

        if (user == null)
            return NotFound("User not found.");

        Users.Remove(user);

        return Ok("User deleted successfully.");
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Test exception");
    }
}