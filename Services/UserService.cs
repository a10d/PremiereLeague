using Neo4j.Driver;
using PremiereLeague.Entities;

namespace PremiereLeague.Services;

public class UserService
{
    private readonly DbService _db;

    public UserService(DbService db)
    {
        _db = db;
    }

    private static User MapUser(IRecord record)
    {
        return new User
        {
            Id = record["id"].As<int>(),
            Name = record["name"].As<string>()
        };
    }

    public List<User> GetUsers()
    {
        return _db.Query("match (u:User) return u.name as name, id(u) as id")
            .Select(MapUser)
            .ToList();
    }


    public User? FindUserByName(string name)
    {
        return _db.Query("match (u:User) where u.name = $name return u.name as name, id(u) as id", new { name })
            .Select(MapUser)
            .ToList()
            .Find(u => u.Name == name);
    }

    public User? FindUserById(int id)
    {
        return _db.Query("match (u:User) where id(u) = $id return u.name as name, id(u) as id", new { id })
            .Select(MapUser)
            .ToList()
            .Find(u => u.Id == id);
    }

    public User CreateUser(string name)
    {
        var result = _db.Query("create (u:User {name: $name}) return u.name as name, id(u) as id",
                new { name })
            .Single();

        return MapUser(result);
    }

    public User UpdateUser(User user)
    {
        var result = _db.Query("match (u:User) where id(u) = $id set u.name = $name return u.name as name, id(u) as id",
                new
                {
                    id = user.Id,
                    name = user.Name
                })
            .Single();

        return MapUser(result);
    }

    public void DeleteUser(User user)
    {
        _db.Query("match (u:User) where id(u) = $id detach delete u", new { id = user.Id });
    }


    public void AddTeamLike(User user, Team team)
    {
        _db.Query("match (u:User), (t:Team) where id(u) = $userId and id(t) = $teamId create (u)-[:LIKES]->(t)",
            new { userId = user.Id, teamId = team.Id });
    }

    public void RemoveTeamLike(User user, Team team)
    {
        _db.Query("match (u:User)-[l:LIKES]->(t:Team) where id(u) = $userId and id(t) = $teamId delete l",
            new { userId = user.Id, teamId = team.Id });
    }
}