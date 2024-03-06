using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PremiereLeague.Entities;
using PremiereLeague.Requests.Team;
using PremiereLeague.Services;

namespace PremiereLeague.Controllers;

[ApiController]
[Route("team/[action]")]
public class TeamController : Controller
{
    private TeamService Service { get; }

    public TeamController(TeamService service)
    {
        Service = service;
    }

    [HttpGet]
    public ActionResult<List<Team>> GetTeams()
    {
        return Ok(Service.GetTeams());
    }

    [HttpGet]
    public ActionResult<Team> GetTeamByName([FromBody] GetTeamByNameRequest r)
    {
        var team = Service.FindTeamByName(r.Name);
        return team == null ? NotFound() : Ok(team);
    }

    [HttpGet]
    public ActionResult<Team> GetTeamById([FromBody] GetTeamByIdRequest r)
    {
        var team = Service.FindTeamById(r.Id);
        return team == null ? NotFound() : Ok(team);
    }

    [HttpPost]
    public ActionResult<Team> AddTeam([FromBody] CreateTeamRequest r)
    {
        return Ok(Service.CreateTeam(r.Name));
    }

    [HttpPut]
    public ActionResult<Team> UpdateTeam([FromBody] UpdateTeamRequest r)
    {
        var team = Service.FindTeamById(r.Id);
        if (team == null) return NotFound();

        team.Name = r.Name;

        return Ok(Service.UpdateTeam(team));
    }

    [HttpDelete]
    public ActionResult DeleteTeam([FromBody] DeleteTeamRequest r)
    {
        var team = Service.FindTeamById(r.Id);
        if (team == null) return NotFound();

        Service.DeleteTeam(team);

        return NoContent();
    }
}
