using Unity.Entities;

struct Config : IComponentData
{
    public Entity playerPrefab;
    public int playerCount;
}