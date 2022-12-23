using Unity.Entities;
using Unity.Mathematics;

struct Player : IComponentData
{
    public float health;
    public float2 direction;
}