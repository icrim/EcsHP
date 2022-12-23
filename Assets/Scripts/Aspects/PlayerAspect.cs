using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

readonly partial struct PlayerAspect : IAspect
{
    readonly Entity self;
    readonly RefRW<Player> Player;
    readonly TransformAspect Transform;

    public int Index
    {
        get => self.Index;
    }

    public float Health
    {
        get => Player.ValueRO.health;
        set => Player.ValueRW.health = value;
    }

    public float2 Direction
    {
        get => Player.ValueRO.direction;
        set => Player.ValueRW.direction = value;
    }

    public float3 Position
    {
        get => Transform.LocalPosition;
        set => Transform.LocalPosition = value;
    }
}
