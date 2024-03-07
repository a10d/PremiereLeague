namespace PremiereLeague.Entities;

public abstract class Entity
{
    public int Id { get; set; }

    public string __type => GetType().Name;
}