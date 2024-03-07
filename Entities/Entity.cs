using System.Text.Json;
using System.Text.Json.Serialization;

namespace PremiereLeague.Entities;

public abstract class Entity
{
    public int Id { get; set; }

    public string __type => GetType().Name;
}
