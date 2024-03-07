using Neo4j.Driver;
using IResult = Neo4j.Driver.IResult;

namespace PremiereLeague.Services;

public class DbService
{
    public DbService()
    {
        Client = GraphDatabase.Driver("bolt://neo4j:7687");
        Client.TryVerifyConnectivityAsync().Wait();
    }

    private IDriver Client { get; }

    public IResult Query(string query, object? parameters = null)
    {
        return Client.Session().Run(query, parameters);
    }
}