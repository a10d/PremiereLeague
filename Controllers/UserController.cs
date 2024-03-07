using Microsoft.AspNetCore.Mvc;
using PremiereLeague.Entities;
using PremiereLeague.Requests.User;
using PremiereLeague.Services;

namespace PremiereLeague.Controllers;

[ApiController]
[Route("user/[action]")]
public class UserController : Controller
{
    private UserService UserService { get; }
    private TeamService TeamService { get; }

    public UserController(UserService userService, TeamService teamService)
    {
        UserService = userService;
        TeamService = teamService;
    }

    [HttpGet]
    public ActionResult<List<User>> GetUsers()
    {
        return Ok(UserService.GetUsers());
    }


    protected User? GetUserFromRequest(GetUserRequest r)
    {
        if (r.Id != null)
        {
            return UserService.FindUserById(r.Id.Value);
        }

        if (r.Name != null)
        {
            return UserService.FindUserByName(r.Name);
        }

        return null;
    }


    [HttpGet]
    public ActionResult<User> GetUser([FromBody] GetUserRequest r)
    {
        var user = GetUserFromRequest(r);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public ActionResult<User> AddUser([FromBody] CreateUserRequest r)
    {
        return Ok(UserService.CreateUser(r.Name));
    }

    [HttpPut]
    public ActionResult<User> UpdateUser([FromBody] UpdateUserRequest r)
    {
        var user = UserService.FindUserById(r.Id);
        if (user == null) return NotFound();

        user.Name = r.Name;

        return Ok(UserService.UpdateUser(user));
    }

    [HttpDelete]
    public ActionResult DeleteUser([FromBody] DeleteUserRequest r)
    {
        var user = UserService.FindUserById(r.Id);
        if (user == null) return NotFound();

        UserService.DeleteUser(user);
        return Ok();
    }

    [HttpGet]
    public ActionResult<List<Team>> GetTeamLikes([FromBody] GetUserRequest r)
    {
        var user = GetUserFromRequest(r);

        if (user == null) return NotFound();

        return Ok(TeamService.GetTeamLikesForUser(user));
    }

    [HttpPost]
    public ActionResult AddTeamLike([FromBody] AddTeamLikeRequest r)
    {
        var user = UserService.FindUserById(r.UserId);
        if (user == null) return NotFound();

        var team = TeamService.FindTeamById(r.TeamId);
        if (team == null) return NotFound();

        UserService.AddTeamLike(user, team);
        return NoContent();
    }

    [HttpPost]
    public ActionResult RemoveTeamLike([FromBody] RemoveTeamLikeRequest r)
    {
        var user = UserService.FindUserById(r.UserId);
        if (user == null) return NotFound();

        var team = TeamService.FindTeamById(r.TeamId);
        if (team == null) return NotFound();

        UserService.RemoveTeamLike(user, team);
        return NoContent();
    }
}
