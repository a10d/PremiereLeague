using System.Text.Json;

namespace PremiereLeague.Entities;

public abstract class Entity
{
    public int Id { get; set; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public override string ToString()
    {
        return ToJson();
    }
}
