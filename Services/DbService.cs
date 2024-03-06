using Neo4j.Driver;
using IResult = Neo4j.Driver.IResult;

namespace PremiereLeague.Services;

public class DbService
{
    private IDriver Client { get; }

    public DbService()
    {
        Client = GraphDatabase.Driver("bolt://localhost:7687");
        Client.TryVerifyConnectivityAsync().Wait();
    }

    public IResult Query(string query, object? parameters = null)
    {
        return Client.Session().Run(query, parameters);
    }
}
