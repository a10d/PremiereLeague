using Neo4j.Driver;
using PremiereLeague.Entities;

namespace PremiereLeague.Services;

public class TeamService
{
    private readonly DbService _db;

    public TeamService(DbService db)
    {
        _db = db;
    }

    private static Team MapTeam(IRecord record)
    {
        return new Team
        {
            Id = record["id"].As<int>(),
            Name = record["name"].As<string>(),
        };
    }


    public List<Team> GetTeams()
    {
        return _db.Query("match (t:Team) return t.name as name, id(t) as id")
            .Select(MapTeam)
            .ToList();
    }

    public Team? FindTeamByName(string name)
    {
        return _db.Query("match (t:Team) where t.name = $name return t.name as name, id(t) as id", new { name })
            .Select(MapTeam)
            .ToList()
            .Find(t => t.Name == name);
    }

    public Team? FindTeamById(int id)
    {
        return _db.Query("match (t:Team) where id(t) = $id return t.name as name, id(t) as id", new { id })
            .Select(MapTeam)
            .ToList()
            .Find(t => t.Id == id);
    }

    public Team CreateTeam(string name)
    {
        var result = _db.Query("create (t:Team {name: $name}) return t.name as name, id(t) as id",
                new { name })
            .Single();

        return MapTeam(result);
    }

    public Team UpdateTeam(Team team)
    {
        var result = _db.Query("match (t:Team) where id(t) = $id set t.name = $name return t.name as name, id(t) as id",
            new
            {
                id = team.Id,
                name = team.Name
            });
        return MapTeam(result.Peek());
    }

    public bool DeleteTeam(Team team)
    {
        _db.Query("match (t:Team) where t.id = $id detach delete t", new
        {
            id = team.Id
        }).Consume();
        return true;
    }
}
