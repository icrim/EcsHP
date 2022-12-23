using Unity.Entities;
using Unity.Transforms;

readonly partial struct HealthAspect : IAspect
{
    readonly RefRW<Health> health;
    readonly RefRO<Parent> parent;

    public float Health
    {
        get => health.ValueRO.health;
        set => health.ValueRW.health = value;
    }

    public Entity Parent => parent.ValueRO.Value;
}